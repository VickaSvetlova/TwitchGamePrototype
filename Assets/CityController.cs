using System;
using System.Collections;
using Script;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;


public class CityController : MonoBehaviour
{
    public event Action<int, int> OnPopulationChange;
    public event Action OnEvacuationComplite;
    public event Action<int, int> OnEvacuationStat;
    [SerializeField] private int populationMax;
    [SerializeField] private Vector2 maxMinEvacuationPiople;
    [SerializeField] private float cooldownEvacuationTransport;
    private int populationCurrent;
    private int evacutionCurrent;
    private IEnumerator coolDownEvacuation;
    public bool EvacuationProcess { private get; set; }
    public GameManager GameManager { private get; set; }


    public float CooldownEvacuationTransport
    {
        get => cooldownEvacuationTransport;
        set => cooldownEvacuationTransport = value;
    }

    private void Start()
    {
        Reset();
        GameManager.OnEvacuationTime += Evacuation;
    }

    public void Reset()
    {
        populationCurrent = populationMax;
        evacutionCurrent = 0;
        OnPopulationChange?.Invoke(populationMax, populationCurrent);
    }

    public void CityDamage(ZombieBase zombie)
    {
        populationCurrent -= zombie.hunger;
        OnPopulationChange?.Invoke(populationMax, populationCurrent);
        if (populationCurrent <= 0)
        {
            OnEvacuationComplite?.Invoke();
        }
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

        if (populationCurrent >= random)
        {
            evacutionCurrent += random;
        }

        if (populationCurrent <= 0)
        {
            StopEvacuation();
            OnEvacuationComplite?.Invoke();
            populationCurrent = 0;
            return;
        }

        OnPopulationChange?.Invoke(populationMax, populationCurrent);
        OnEvacuationStat?.Invoke(populationMax, evacutionCurrent);
    }

    private void OnDestroy()
    {
        GameManager.OnEvacuationTime -= Evacuation;
    }
}