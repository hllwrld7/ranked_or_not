using Newtonsoft.Json;
using RankedOrNot.Core;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace RankedOrNot.Core
{
    public class RiotAPICommunication: IRiotAPICommunication
    {
        static HttpClient _client;
        string _gameName;
        string _tagline;
        List<string> RANKED_QUEUE_IDS = new List<string> { "420", "440", "1100" }; 

        public RiotAPICommunication(string apiToken, string gameName, string tagLine)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("X-Riot-Token", apiToken);
            _gameName = gameName;
            _tagline = tagLine;
            //SetPuuid(gameName, tagLine);
        }

        private async Task<string> GetPuuid()
        {
            HttpResponseMessage response = await _client.GetAsync(
    $"https://europe.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{_gameName}/{_tagline}");

            if (!response.IsSuccessStatusCode) return "";

            var responseString = await response.Content.ReadAsStringAsync();

            if (responseString == null) return "";
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(responseString);

            if(obj == null) return "";
            return obj.puuid;
        }
        public async Task<MatchInfo> GetMatchInfo()
        {
            var puuid = await GetPuuid();

            HttpResponseMessage response = await _client.GetAsync(
                $"https://euw1.api.riotgames.com/lol/spectator/tft/v5/active-games/by-puuid/{puuid}");

            if (!response.IsSuccessStatusCode)
            {
                response = await _client.GetAsync(
                $"https://euw1.api.riotgames.com/lol/spectator/v5/active-games/by-summoner/{puuid}");

                if (!response.IsSuccessStatusCode)
                    return new MatchInfo()
                    {
                        isOngoing = false
                    };
            }

            var responseString = await response.Content.ReadAsStringAsync();

            if (responseString == null)
                throw new Exception();

            dynamic obj = JsonConvert.DeserializeObject<dynamic>(responseString);

            if (obj == null)
                throw new Exception();

            return new MatchInfo()
            {
                isOngoing = true,
                isRanked = RANKED_QUEUE_IDS.Contains(obj.gameQueueConfigId.ToString())
            };
        }

    }
}
