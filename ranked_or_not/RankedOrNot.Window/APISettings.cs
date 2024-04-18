using RankedOrNot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankedOrNot.Window
{
    internal class APISettings: IAPISettings
    {
        public string LeagueApiKey { get; set; } = "";
        public string TftApiKey { get; set; } = "";
        public string LeagueName { get; set; } = "helloworld777";
        public string Tagline { get; set; } = "euw";
    }
}
