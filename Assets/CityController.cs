using System;
using System.Collections;
using Script;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;


public class CityController : MonoBehaviour
{
    public event Action<int, int> PopulationChange;
    public event Action EvacuationComplite;
    [SerializeField] private int populationMax;
    [SerializeField] private Vector2 maxMinEvacuationPiople;
    [SerializeField] private float cooldownEvacuationTransport;
    private int populationCurrent;
    private IEnumerator coolDownEvacuation;


    public float CooldownEvacuationTransport
    {
        get => cooldownEvacuationTransport;
        set => cooldownEvacuationTransport = value;
    }

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        populationCurrent = populationMax;
        PopulationChange?.Invoke(populationMax, populationCurrent);
    }

    public void CityDamage(ZombieBase zombie)
    {
        populationCurrent -= zombie.hunger;
        PopulationChange?.Invoke(populationMax, populationCurrent);
        if (populationCurrent <= 0)
        {
            EvacuationComplite?.Invoke();
        }
    }

    public void StartEvacuation()
    {
        if (coolDownEvacuation != null) return;
        coolDownEvacuation = CoolDownEvacuation(CooldownEvacuationTransport);
        StartCoroutine(coolDownEvacuation);
    }

    public void StopEvacuation()
    {
        if (coolDownEvacuation == null) return;
        StopCoroutine(coolDownEvacuation);
    }

    private IEnumerator CoolDownEvacuation(float timer)
    {
        while (coolDownEvacuation != null)
        {
            yield return new WaitForSeconds(timer);
            Evacuation();
        }
    }

    private void Evacuation()
    {
        populationCurrent -= (int) Random.Range(maxMinEvacuationPiople.x, maxMinEvacuationPiople.y);

        if (populationCurrent <= 0)
        {
            StopEvacuation();
            EvacuationComplite?.Invoke();
        }

        PopulationChange?.Invoke(populationMax, populationCurrent);
    }
}