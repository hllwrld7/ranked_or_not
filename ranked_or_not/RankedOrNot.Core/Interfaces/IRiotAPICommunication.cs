using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankedOrNot.Core.Interfaces
{
    public interface IRiotAPICommunication
    {
        public Task<string> GetPuuid(string apiKey);
        public Task<APIRequestResponse> GetCurrentTftGame();
        public Task<APIRequestResponse> GetCurrentLeagueGame();
    }
}
