using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;
using Random = UnityEngine.Random;


public class CityController : MonoBehaviour
{
    public event Action<GameStatistic> OnPopulationChange;
    [SerializeField] private int populationMax;
    [SerializeField] private Vector2 maxMinEvacuationPiople;
    private GameStatistic _statisticWave;
    private List<GameStatistic> _gameStatistics = new List<GameStatistic>();

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        _gameStatistics.Clear();
        _statisticWave = new GameStatistic(populationMax, populationMax, _gameStatistics);
        StartNextWave();
    }

    public void StartNextWave()
    {
        var current = 0;

        current = _statisticWave != null ? _statisticWave.populationCurrent : populationMax;

        _statisticWave = new GameStatistic(populationMax, current, _gameStatistics);

        _gameStatistics.Add(_statisticWave);
        OnPopulationChange?.Invoke(_statisticWave);
    }

    public void CityDamage(ZombieBase zombie)
    {
        _statisticWave.Eating(zombie.hunger);
        OnPopulationChange?.Invoke(_statisticWave);
    }


    public void Evacuation()
    {
        var random = (int) Random.Range(maxMinEvacuationPiople.x, maxMinEvacuationPiople.y);

        _statisticWave.Evacuation(random);
        OnPopulationChange?.Invoke(_statisticWave);
    }
}

public class GameStatistic
{
    public int populationMax { get; private set; }
    public int populationCurrent { get; private set; }
    public int populationEating { get; private set; }
    public int populationEvacuation { get; private set; }

    public List<GameStatistic> gameStatistics { get; private set; }


    public GameStatistic(int populationMax, int populationCurrent, List<GameStatistic> gameStatistics)
    {
        this.populationMax = populationMax;
        this.populationCurrent = populationCurrent;
        this.gameStatistics = gameStatistics;
    }


    public void Eating(int people)
    {
        var food = people > populationCurrent ? populationCurrent : people;
        populationCurrent -= food;
        populationEating += food;
    }

    public void Evacuation(int people)
    {
        var evacuationUnit = people > populationCurrent ? populationCurrent : people;
        populationCurrent -= evacuationUnit;
        populationEvacuation += evacuationUnit;
    }

    public bool CityIsEmpty()
    {
        return populationCurrent == 0;
    }

    public GameStatistic GetGameStatistic()
    {
        gameStatistics.RemoveAt(gameStatistics.Count - 1);
        foreach (var statistic in gameStatistics)
        {
            populationEating += statistic.populationEating;
            populationEvacuation += statistic.populationEvacuation;
            populationMax = statistic.populationMax;
        }

        return this;
    }
}