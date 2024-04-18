using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankedOrNot.Core.Interfaces
{
    public interface IAPISettings
    {
        public string LeagueName { get; set; }
        public string Tagline { get; set; }
        public string LeagueApiKey { get; set; }
        public string TftApiKey { get; set; }
    }
}
