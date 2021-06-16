using System.Collections.Generic;
using Script;
using Script.interfaces;


public class SurvivorStatisticGame

{
    public List<ISurvivorStatistic> StatisticsGame { get; set; }

    public SurvivorStatisticGame(List<ISurvivorStatistic> statisticsGame)
    {
        StatisticsGame = statisticsGame;
    }

    public ISurvivorStatistic GetSumStatisticGame()
    {
        ISurvivorStatistic tempStat = new SurvivorStatisticWave();
        foreach (var statistic in StatisticsGame)
        {
            tempStat.AimHits += statistic.AimHits;
            tempStat.HeadHits += statistic.HeadHits;
            tempStat.TotalKills += statistic.TotalKills;
            tempStat.TotalShoot += statistic.TotalShoot;
        }

        return tempStat;
    }
}