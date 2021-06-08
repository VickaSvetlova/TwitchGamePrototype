using System;
using Script;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject screenGameOver;
    [SerializeField] private GameObject screenStatistics;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Slider slider;
    [SerializeField] private Text populationCount;
    [SerializeField] private GameObject UINamePrefab;

    public void SetPopulation(int populationMax, int populationCurrent)
    {
        slider.maxValue = populationMax;
        slider.value = populationCurrent;
        populationCount.text = populationCurrent.ToString();
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