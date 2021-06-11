using Script.interfaces;

namespace Script
{
    public class SurvivorStatisticGame : ISurvivorStatistic

    {
        public int TotalShoot { get; set; }
        public int TotalHits { get; set; }
        public int AimHits { get; set; }
        public int HeadHits { get; set; }
        public int TotalKills { get; set; }
        
    }
}