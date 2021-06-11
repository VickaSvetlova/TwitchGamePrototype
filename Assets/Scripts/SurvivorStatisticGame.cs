using System.Collections.Generic;
using Script.interfaces;

namespace Script
{
    public class SurvivorStatisticGame

    {
        public List<ISurvivorStatistic> StatisticsGame;

        public SurvivorStatisticGame(List<ISurvivorStatistic> statisticsGame)
        {
            StatisticsGame = statisticsGame;
        }
    }
}