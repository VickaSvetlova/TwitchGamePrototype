using System;
using System.Collections;
using Script;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    [SerializeField] private GameObject screenGameOver;
    [SerializeField] private Text populationMax;
    [SerializeField] private Text populationEvacuation;
    [SerializeField] private Text populationEating;

    [SerializeField] private GameObject screenStatistics;
    [SerializeField] private Text StatPopulationMax;
    [SerializeField] private Text StatPopulationEvacuation;
    [SerializeField] private Text StatePpulationEating;
    
    [SerializeField] private Slider sliderEvacuation;
    [SerializeField] private Slider sliderPopulation;
    [SerializeField] private GameObject UINamePrefab;

    private bool enable;
    public void SetStatistic(IStatistic statistic)
    {
        if (statistic.CityIsEmpty())
        {
            if(enable) return; //костыль 
            StatisticGameOver(statistic);//всех съели
            enable = true;
        }
        else
        {
            enable = false;
            StatisticBetweenWave(statistic);
        }

        StatisticInGame(statistic);
    }

    public void StatisticInGame(IStatistic statistic)
    {
        sliderPopulation.maxValue = statistic.populationMax;
        sliderPopulation.value = statistic.populationCurrent;
        sliderEvacuation.maxValue = statistic.populationMax;
        sliderEvacuation.value = statistic.populationEvacuation;
    }

    void StatisticGameOver(IStatistic statistic)
    {
        populationMax.text = statistic.populationMax.ToString();
        populationEvacuation.text = statistic.populationEvacuation.ToString();
        populationEating.text = statistic.populationEating.ToString();
    }

    void StatisticBetweenWave(IStatistic statistic)
    {
        StatPopulationMax.text = statistic.populationCurrent.ToString();
        StatPopulationEvacuation.text = statistic.populationEvacuation.ToString();
        StatePpulationEating.text = statistic.populationEating.ToString();
    }

    public void CreateUIName(ZombieBase zombieBase)
    {
        var tempNamePrefab = Instantiate(UINamePrefab);

        tempNamePrefab.transform.SetParent(_canvas.transform);

        var temp = tempNamePrefab.GetComponent<UIName>();
        zombieBase.UIName = temp;
        temp.TransformFolowObject = zombieBase.transform;
        temp.Name = zombieBase.Name;
    }

    public void GameOverWindow(bool state)
    {
        screenGameOver.SetActive(state);
    }

    public void StatisticsWindow(bool state)
    {
        screenStatistics.SetActive(state);
    }
}