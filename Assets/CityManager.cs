using System;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    public Action<int, int> PopulationChange;
    [SerializeField] private int populationMax;
    private int populationCurrent;
    [SerializeField] private float cooldownEvacuationTransport;
    [SerializeField] private UIController uiController;

    private void Awake()
    {
        populationCurrent = populationMax;
        uiController.SubscribeManager(this);
        PopulationChange?.Invoke(populationMax,populationCurrent);
    }

    public void CityDamage(int killPiople)
    {
        populationCurrent -= killPiople;
        PopulationChange?.Invoke(populationMax,populationCurrent);
        if (populationCurrent <= 0)
        {
            //city destroy
            return;
        }
        
    }
}