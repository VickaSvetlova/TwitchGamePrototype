using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class CityController : MonoBehaviour
{
    public Action<int, int> PopulationChange;
    public Action EvacuationComplite;
    [SerializeField] private int populationMax;
    [SerializeField] [Min(0)] private Vector2 maxMinEvacuationPiople;

    [SerializeField] private float cooldownEvacuationTransport;
    [SerializeField] private UIController uiController;

    private int populationCurrent;
    private IEnumerator coolDownEvacuation;

    public float CooldownEvacuationTransport
    {
        get => cooldownEvacuationTransport;
        set => cooldownEvacuationTransport = value;
    }

    private void Awake()
    {
        populationCurrent = populationMax;
        uiController.SubscribeManager(this);
        PopulationChange?.Invoke(populationMax, populationCurrent);
    }

    public void CityDamage(int killPiople)
    {
        populationCurrent -= killPiople;
        PopulationChange?.Invoke(populationMax, populationCurrent);
        if (populationCurrent <= 0)
        {
            //city destroy
            return;
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