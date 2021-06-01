using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityManager : MonoBehaviour
{
    public Action<float, float> PopulationChange;
    [SerializeField] private float populationMax;
    private float populationCurrent;
    [SerializeField] private float cooldownEvacuationTransport;
    [SerializeField] private UIController uiController;

    private void Awake()
    {
        uiController.SubscribeManager(this);
    }
}

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void SetPopulation(float populationMax, float populationCurrent)
    {
        slider.maxValue = populationMax;
        slider.value = populationCurrent;
    }

    public void SubscribeManager(CityManager cityManager)
    {
        cityManager.PopulationChange += SetPopulation;
    }
}