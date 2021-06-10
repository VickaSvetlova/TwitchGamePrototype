using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;


public enum GameState
{
    idle,
    game,
    statBetwinWave,
    gameOver
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private ZombieController zombieController;
    [SerializeField] private CityController cityController;
    [SerializeField] private UIController uiController;
    [SerializeField] private ChatController chatController;
    [SerializeField] private CharController charController;

    [SerializeField] private float coolDawnNextEvacuation;

    private IEnumerator coolDown;
    private IEnumerator coolDownEvacuation;
    private bool EvacuationTimerStatus;


    private GameState _previusState;
    [SerializeField] private float nextGameTimer;
    [SerializeField] private float nextWaveTimer;


    private void Awake()
    {
        _previusState = GameState.gameOver;
        SetState(GameState.idle);

        chatController.CharController = charController;
        charController.ZombieProvider = zombieController;

        SubscribesControllers();
    }

    private void SubscribesControllers()
    {
        zombieController.OnCurrentWaveIsOut += () => { SetState(GameState.statBetwinWave); };
        zombieController.OnZombieCreated += (zombie) => { uiController.CreateUIName(zombie); };
        zombieController.OnZombieReachedCity += (zombie) => { cityController.CityDamage(zombie); };
        chatController.OnUserAppeared += () => { SetState(GameState.game); };
        cityController.OnPopulationChange += (statistic) =>
        {
            if (statistic.CityIsEmpty())
            {
                SetState(GameState.gameOver);
            }

            uiController.SetStatistic(statistic);
        };
    }

    public void SetState(GameState _state)
    {
        switch (_state)
        {
            case GameState.idle:
                if (_previusState == GameState.gameOver)
                {
                    Reset();
                    _previusState = GameState.idle;
                    chatController.ONChatEnable = true;
                    uiController.GameOverWindow(false);
                    uiController.StatisticsWindow(false);
                }

                break;
            case GameState.game:
                if (_previusState == GameState.idle || _previusState == GameState.statBetwinWave)
                {
                    StartEvacuation(coolDawnNextEvacuation);
                    _previusState = GameState.game;
                    chatController.ONChatEnable = true;
                    zombieController.NextWave();
                    uiController.StatisticsWindow(false);
                }

                break;
            case GameState.statBetwinWave:
                if (_previusState == GameState.game)
                {
                    StopEvacuation();
                    _previusState = GameState.statBetwinWave;
                    chatController.ONChatEnable = false;
                    uiController.StatisticsWindow(true);
                    StartCooldown(nextWaveTimer, GameState.game);
                }

                break;
            case GameState.gameOver:
                if (_previusState == GameState.game)
                {
                    StopEvacuation();
                    _previusState = GameState.gameOver;
                    chatController.ONChatEnable = false;
                    uiController.GameOverWindow(true);
                    zombieController.StopGame();
                    StartCooldown(nextGameTimer, GameState.idle);
                }

                break;
        }
    }

    private void StartEvacuation(float time)
    {
        if (coolDownEvacuation == null)
            coolDownEvacuation = CoolDownEvacuation(time);
        StartCoroutine(coolDownEvacuation);
    }

    private void StopEvacuation()
    {
        if (coolDownEvacuation != null)
            StopCoroutine(coolDownEvacuation);
    }

    private void Reset()
    {
        cityController.Reset();
        chatController.Reset();
        zombieController.Reset();
    }

    private void StartCooldown(float time, GameState stateNext)
    {
        if (coolDown == null)
        {
            coolDown = CoolDown(time, stateNext);
            StartCoroutine(coolDown);
        }
    }

    private IEnumerator CoolDown(float time, GameState stateNext)
    {
        yield return new WaitForSeconds(time);
        SetState(stateNext);
        coolDown = null;
    }

    private IEnumerator CoolDownEvacuation(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            cityController.Evacuation();
        }
    }
}