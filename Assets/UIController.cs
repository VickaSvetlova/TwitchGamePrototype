using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Text populationCount;

    public void SetPopulation(int populationMax, int populationCurrent)
    {
        slider.maxValue = populationMax;
        slider.value = populationCurrent;
        populationCount.text = populationCurrent.ToString();
    }
    public void SubscribeManager(CityManager cityManager)
    {
        cityManager.PopulationChange += SetPopulation;
    }
}