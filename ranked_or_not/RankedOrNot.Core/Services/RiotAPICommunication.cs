using Newtonsoft.Json;
using RankedOrNot.Core.Interfaces;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace RankedOrNot.Core.Services
{
    public class RiotAPICommunication : IRiotAPICommunication
    {
        private readonly static HttpClient _client = new HttpClient();
        private readonly IAPISettings _settings;

        public RiotAPICommunication(IAPISettings settings)
        {
            _settings = settings;
        }

        public async Task<string> GetPuuid(string apiKey)
        {
            HttpResponseMessage response = await _client.GetAsync(
    $"https://europe.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{_settings.LeagueName}/{_settings.Tagline}?api_key={apiKey}");

            if (!response.IsSuccessStatusCode) return "";

            var responseString = await response.Content.ReadAsStringAsync();

            if (responseString == null) return "";
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(responseString);

            if (obj == null) return "";
            return obj.puuid;
        }

        public async Task<APIRequestResponse> GetCurrentTftGame()
        {
            var apiRequestResponse = new APIRequestResponse();

            var puuid = await GetPuuid(_settings.TftApiKey);

            HttpResponseMessage response = await _client.GetAsync(
                $"https://euw1.api.riotgames.com/lol/spectator/tft/v5/active-games/by-puuid/{puuid}?api_key={_settings.TftApiKey}");

            apiRequestResponse.ResponseCode = Convert.ToInt16(response.StatusCode);

            apiRequestResponse.ResponseText = await response.Content.ReadAsStringAsync();

            return apiRequestResponse;
        }

        public async Task<APIRequestResponse> GetCurrentLeagueGame()
        {
            var apiRequestResponse = new APIRequestResponse();

            var puuid = await GetPuuid(_settings.LeagueApiKey);

            HttpResponseMessage response = await _client.GetAsync(
                $"https://euw1.api.riotgames.com/lol/spectator/v5/active-games/by-summoner/{puuid}?api_key={_settings.LeagueApiKey}");

            apiRequestResponse.ResponseCode = Convert.ToInt16(response.StatusCode);

            apiRequestResponse.ResponseText = await response.Content.ReadAsStringAsync();

            return apiRequestResponse;
        }
    }
}
