using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    [SerializeField] private float coolDawnNextWaveTime;

    private IEnumerator coolDownNextWave;


    private GameState _previusState;

    private void Awake()
    {
        _previusState = GameState.gameOver;
        SetState(_previusState);
        chatController.Manager = this;
    }

    public void SetState(GameState _state)
    {
        switch (_state)
        {
            case GameState.idle:
                if (_previusState == GameState.gameOver)
                {
                    _previusState = GameState.idle;
                    chatController.ONChatEnable = true;
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
                    StartCooldownNextWave();
                }

                break;
            case GameState.gameOver:
                if (_previusState == GameState.game)
                {
                    _previusState = GameState.gameOver;
                    chatController.ONChatEnable = false;
                    zombieController.StateManager(false);
                }

                break;
        }
    }

    private void StartCooldownNextWave()
    {
        if (coolDownNextWave != null)
            coolDownNextWave = CoolDownNexWave();
    }

    private IEnumerator CoolDownNexWave()
    {
        yield return new WaitForSeconds(coolDawnNextWaveTime);
        SetState(GameState.game);
    }
}