using System;
using System.Collections;
using Script;
using UnityEngine;
using Random = UnityEngine.Random;


public class CityController : MonoBehaviour
{
    public event Action<int, int> OnPopulationChange;
    public event Action<Statistic> OnEvacuationComplite;
    public event Action<int, int> OnEvacuationStat;
    [SerializeField] private int populationMax;
    [SerializeField] private Vector2 maxMinEvacuationPiople;
    private int populationCurrent;
    private int populationEating;
    private int populationEvacuation;

    private IEnumerator coolDownEvacuation;
    public GameManager GameManager { private get; set; }

    private void Start()
    {
        Reset();
        GameManager.OnEvacuationTime += Evacuation;
    }

    public void Reset()
    {
        populationCurrent = populationMax;
        populationEvacuation = 0;
        populationEating = 0;
        OnEvacuationStat?.Invoke(populationMax, 0);
        OnPopulationChange?.Invoke(populationMax, populationCurrent);
    }

    public void CityDamage(ZombieBase zombie)
    {
        if (populationMax > 0)
        {
            populationCurrent -= zombie.hunger;
            populationEating += zombie.hunger;
        }

        if (populationCurrent <= 0)
        {
            OnEvacuationComplite?.Invoke(CreateStatistic(populationMax, populationEvacuation, populationEating));
            populationCurrent = 0;
        }

        OnPopulationChange?.Invoke(populationMax, populationCurrent);
    }

    private Statistic CreateStatistic(int max, int populationEvacuation, int populationEating)
    {
        var stat = new Statistic(max, populationEvacuation, populationEating);
        return stat;
    }

    public void StopEvacuation()
    {
        if (coolDownEvacuation == null) return;
        StopCoroutine(coolDownEvacuation);
    }

    private void Evacuation()
    {
        var random = (int) Random.Range(maxMinEvacuationPiople.x, maxMinEvacuationPiople.y);

        populationCurrent -= random;
        populationEvacuation += random;

        if (populationCurrent <= 0)
        {
            populationCurrent = 0;
            OnEvacuationComplite?.Invoke(CreateStatistic(populationMax, populationEvacuation, populationEating));
            StopEvacuation();
            return;
        }

        OnPopulationChange?.Invoke(populationMax, populationCurrent);
        OnEvacuationStat?.Invoke(populationMax, populationEvacuation);
    }

    private void OnDestroy()
    {
        GameManager.OnEvacuationTime -= Evacuation;
    }
}