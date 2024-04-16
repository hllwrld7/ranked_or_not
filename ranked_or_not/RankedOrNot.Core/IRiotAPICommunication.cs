using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankedOrNot.Core
{
    public interface IRiotAPICommunication
    {
        Task<MatchInfo> GetMatchInfo();
    }
}
