using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;


public enum GameState
{
    idle,
    game,
    statistic,
    gameOver
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private ZombieController zombieController;
    [SerializeField] private CityController cityController;
    [SerializeField] private UIController uiController;
    [SerializeField] private ChatController chatController;
    [SerializeField] private CharController charController;

    [SerializeField] private float coolDawnNextWaveTime;

    private IEnumerator coolDown;


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
        zombieController.OnZombieCreated += (zombie) => { uiController.CreateUIName(zombie); };
        zombieController.OnZombieReachedCity += (zombie) => { cityController.CityDamage(zombie); };
        chatController.OnUserAppeared += () => { SetState(GameState.game); };
        cityController.PopulationChange += (max, curr) => { uiController.SetPopulation(max, curr); };
        cityController.EvacuationComplite += () => { SetState(GameState.gameOver); };
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
                    zombieController.StateManager(false);
                    uiController.GameOver(false);
                }

                break;
            case GameState.game:
                if (_previusState == GameState.idle || _previusState == GameState.statistic)
                {
                    _previusState = GameState.game;
                    chatController.ONChatEnable = true;
                    zombieController.StateManager(true);
                }

                break;
            case GameState.statistic:
                if (_previusState == GameState.game)
                {
                    chatController.ONChatEnable = false;
                    zombieController.StateManager(false);
                    StartCooldown(nextWaveTimer, GameState.game);
                }

                break;
            case GameState.gameOver:
                if (_previusState == GameState.game)
                {
                    _previusState = GameState.gameOver;
                    chatController.ONChatEnable = false;
                    zombieController.StateManager(false);
                    uiController.GameOver(true);
                    StartCooldown(nextGameTimer, GameState.idle);
                }

                break;
        }
    }

    private void Reset()
    {
        cityController.Reset();
        chatController.Reset();
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
}