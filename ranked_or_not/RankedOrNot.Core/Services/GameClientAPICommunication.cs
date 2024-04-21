using Newtonsoft.Json;
using RankedOrNot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace RankedOrNot.Core.Services
{
    public class GameClientAPICommunication : IGameClientAPICommunication
    {

        public async Task<string> GetCurrentGameMode()
        {
            HttpResponseMessage response;

            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient(httpClientHandler) { BaseAddress = new Uri("https://127.0.0.1:2999") })
                {
                    try
                    {
                        response = await client.GetAsync(
                "/liveclientdata/gamestats");
                    }
                    catch(Exception ex)
                    {
                        return "";
                    }
                }
            }

            if (!response.IsSuccessStatusCode)
                return "";

            var responseString = await response.Content.ReadAsStringAsync();

            if (responseString == null)
                return "";

            var obj = JsonConvert.DeserializeObject<dynamic>(responseString);

            return obj == null ? "" : obj.gameMode;
        }
    }
}
