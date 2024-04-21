using Newtonsoft.Json;
using RankedOrNot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankedOrNot.Core.Services
{
    public class MatchInfoHelper : IMatchInfoHelper
    {
        private readonly List<string> RANKED_QUEUE_IDS = new List<string> { "420", "440", "1100" };
        private readonly IRiotAPICommunication _riotAPICommunication;
        private readonly IGameClientAPICommunication _gameClientAPICommunication;

        public MatchInfoHelper(IRiotAPICommunication riotAPICommunication, IGameClientAPICommunication gameClientAPICommunication)
        {
            _riotAPICommunication = riotAPICommunication;
            _gameClientAPICommunication = gameClientAPICommunication;
        }

        public async Task<MatchInfo> GetMatchInfo()
        {
            var matchInfo = new MatchInfo();

            APIRequestResponse requestResponse;
            var currentGameMode = await _gameClientAPICommunication.GetCurrentGameMode();
            if (string.IsNullOrEmpty(currentGameMode))
                return matchInfo;
            else if (currentGameMode == "TFT")
                requestResponse = await _riotAPICommunication.GetCurrentTftGame();
            else
                requestResponse = await _riotAPICommunication.GetCurrentLeagueGame();

            if (requestResponse.ResponseCode != 404 && requestResponse.ResponseCode != 200)
                throw new Exception($"Request response code = {requestResponse.ResponseCode}");

            matchInfo.IsOngoing = requestResponse.ResponseCode == 200;

            if (!matchInfo.IsOngoing || requestResponse.ResponseText == null)
                return matchInfo;

            var gameQueueId = GetGameQueueIdFromResponseString(requestResponse.ResponseText);
            matchInfo.IsRanked = RANKED_QUEUE_IDS.Contains(gameQueueId);

            return matchInfo;
        }

        private string GetGameQueueIdFromResponseString(string responseString)
        {
            var obj = JsonConvert.DeserializeObject<dynamic>(responseString);

            if (obj == null)
                throw new Exception("Unable to get object from the response string");

            return obj.gameQueueConfigId.ToString();
        }
    }
}
