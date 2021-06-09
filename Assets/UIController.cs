using System;
using Script;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject screenGameOver;
    [SerializeField] private GameObject screenStatistics;
    [SerializeField] private Canvas _canvas; 
    [SerializeField] private Slider sliderEvacuation;
    [SerializeField] private Slider sliderPopulation;
    [SerializeField] private Text populationCount;
    [SerializeField] private GameObject UINamePrefab;

    public void SetPopulation(int populationMax, int populationCurrent)
    {
        sliderPopulation.maxValue = populationMax;
        sliderPopulation.value = populationCurrent;
        populationCount.text = populationCurrent.ToString();
    }
    public void SetEvacuation(int populationMax, int evacuationCurrent)
    {
        sliderEvacuation.maxValue = populationMax;
        sliderEvacuation.value = evacuationCurrent;
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

    public void GameOver(bool state)
    {
        screenGameOver.SetActive(state);
    }

    public void Statistics(bool state)
    {
        screenStatistics.SetActive(state);
    }
}