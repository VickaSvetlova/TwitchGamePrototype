using Script;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject screenGameOver;
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

    public void SubscribeManager(CityController cityController)
    {
        cityController.PopulationChange += SetPopulation;
    }

    public void CreateUIName(string tempName, ZombieBase zombieBase)
    {
        var tempNamePrefab = Instantiate(UINamePrefab);

        tempNamePrefab.transform.SetParent(_canvas.transform);

        var temp = tempNamePrefab.GetComponent<UIName>();
        zombieBase.UIName = temp;
        temp.TransformFolowObject = zombieBase.transform;
        temp.Name = tempName;
    }
}