using System;
using System.Collections;
using Script;
using UnityEngine;
using Random = UnityEngine.Random;


public class CityController : MonoBehaviour
{
    public event Action<WaveStatistic> OnPopulationChange;
    [SerializeField] private int populationMax;
    [SerializeField] private Vector2 maxMinEvacuationPiople;
    private WaveStatistic _statistic;

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        _statistic = new WaveStatistic(populationMax, populationMax);
        OnPopulationChange?.Invoke(_statistic);
    }

    public void CityDamage(ZombieBase zombie)
    {
        _statistic.Eating(zombie.hunger);
        OnPopulationChange?.Invoke(_statistic);
    }


    public void Evacuation()
    {
        var random = (int) Random.Range(maxMinEvacuationPiople.x, maxMinEvacuationPiople.y);

        _statistic.Evacuation(random);
        OnPopulationChange?.Invoke(_statistic);
    }
}

public class WaveStatistic
{
    public int populationMax { get; private set; }
    public int populationCurrent { get; private set; }
    public int populationEating { get; private set; }
    public int populationEvacuation { get; private set; }

    public WaveStatistic(int populationMax, int populationCurrent)
    {
        this.populationMax = populationMax;
        this.populationCurrent = populationCurrent;
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