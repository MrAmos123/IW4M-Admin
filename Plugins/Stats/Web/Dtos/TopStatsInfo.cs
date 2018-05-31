﻿using SharedLibraryCore.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace IW4MAdmin.Plugins.Stats.Web.Dtos
{
    public class TopStatsInfo : SharedInfo
    {
        public int Ranking { get; set; }
        public string Name { get; set; }
        public int ClientId { get; set; }
        public double KDR { get; set; }
        public double Performance { get; set; }
        public string TimePlayed { get; set; }
        public string LastSeen { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public List<double> PerformanceHistory { get; set; }
    }
}
