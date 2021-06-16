using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;
using Random = UnityEngine.Random;


public class CityController : MonoBehaviour
{
    public event Action<IStatistic> OnPopulationChange;
    [SerializeField] private int populationMax;
    [SerializeField] private Vector2 maxMinEvacuationPiople;
    private WaveStatistic _statisticWave;
    private List<WaveStatistic> _gameStatistics = new List<WaveStatistic>();

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        _gameStatistics.Clear();
        _statisticWave = new WaveStatistic(populationMax, populationMax, _gameStatistics);
        NextWave();
    }

    public void NextWave()
    {
        var current = 0;

        current = _statisticWave != null ? _statisticWave.populationCurrent : populationMax;

        _statisticWave = new WaveStatistic(populationMax, current, _gameStatistics);

        _gameStatistics.Add(_statisticWave);
        CheckStatistics();
    }

    public void CityDamage(ZombieBase zombie)
    {
        _statisticWave.Eating(zombie.hunger);
        CheckStatistics();
    }


    public void Evacuation()
    {
        var random = (int) Random.Range(maxMinEvacuationPiople.x, maxMinEvacuationPiople.y);

        _statisticWave.Evacuation(random);
        CheckStatistics();
    }

    private void CheckStatistics()
    {
        if (_statisticWave.CityIsEmpty())
        {
            OnPopulationChange?.Invoke(new FullStatistics(_gameStatistics));
            return;
        }

        OnPopulationChange?.Invoke(_statisticWave);
    }
}

public class WaveStatistic : IStatistic
{
    public int populationMax { get; private set; }
    public int populationCurrent { get; private set; }
    public int populationEating { get; private set; }
    public int populationEvacuation { get; private set; }

    public List<WaveStatistic> gameStatistics { get; private set; }


    public WaveStatistic(int populationMax, int populationCurrent, List<WaveStatistic> gameStatistics)
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
}

public class FullStatistics : IStatistic
{
    public int populationMax { get; }
    public int populationCurrent { get; }
    public int populationEating { get; }
    public int populationEvacuation { get; }

    public FullStatistics(List<WaveStatistic> waveStatistics)
    {
        foreach (var statistic in waveStatistics)
        {
            populationEating += statistic.populationEating;
            populationEvacuation += statistic.populationEvacuation;
        }

        populationMax = populationEating + populationEvacuation;
    }

    public bool CityIsEmpty()
    {
        return true;
    }
}

public interface IStatistic
{
    public int populationMax { get; }
    public int populationCurrent { get; }
    public int populationEating { get; }
    public int populationEvacuation { get; }

    public bool CityIsEmpty();
}