using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script;
using twitch.game.Iface;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    [SerializeField] private Text chat;

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
            if (enable) return; //костыль 
            StatisticGameOver(statistic); //всех съели
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

    public void SendChat(string text)
    {
        chat.text += text;
    }

    void StatisticBetweenWave(IStatistic statistic)
    {
        StatPopulationMax.text = statistic.populationCurrent.ToString();
        StatPopulationEvacuation.text = statistic.populationEvacuation.ToString();
        StatePpulationEating.text = statistic.populationEating.ToString();
    }

    public void CreateUIName(IName name)
    {
        var tempNamePrefab = Instantiate(UINamePrefab);

        tempNamePrefab.transform.SetParent(_canvas.transform);
        var temp = tempNamePrefab.GetComponent<UIName>();
        temp.Name = name.CharacterName;
        temp.TransformFolowObject = name.CharacterGameObject.transform;
        temp.Name = name.CharacterName;
    }

    public void GameOverWindow(bool state)
    {
        screenGameOver.SetActive(state);
    }

    public void StatisticsWindow(bool state)
    {
        screenStatistics.SetActive(state);
    }

    public void SendStatistic(Dictionary<User, SurvivorStatisticGame> sendStatisticUI, bool isGameOut)
    {
        foreach (var user in sendStatisticUI.Keys)
        {
            switch (isGameOut)
            {
                case true:
                    var tempStat = sendStatisticUI[user].GetSumStatisticGame();
                    SendChat("\n" + string.Format(
                        "statistic Survivor on Round: {0} TotallShoot: {1} Headshoot: {2} aimShoot: {3} Totall kill {4}",
                        user.NameUser, tempStat.TotalShoot, tempStat.HeadHits, tempStat.AimHits, tempStat.TotalKills));

                    break;
                case false:
                    var statInWave = sendStatisticUI[user]
                        .StatisticsGame[sendStatisticUI[user].StatisticsGame.Count - 1];
                    SendChat("\n" + string.Format(
                        " statistic Survivor on Wave : {0} TotallShoot: {1} Headshoot: {2} aimShoot: {3} Totall kill {4}",
                        user.NameUser, statInWave.TotalShoot, statInWave.HeadHits, statInWave.AimHits,
                        statInWave.TotalKills));
                    break;
            }
        }
    }

    public void Reset()
    {
        chat.text = "";
    }
}