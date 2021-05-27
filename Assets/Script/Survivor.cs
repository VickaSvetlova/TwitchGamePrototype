using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script;
using UnityEngine;


public class Survivor : MonoBehaviour

{
    private BaseWeapon _weapon;
    public ChatController.User user;
    private GameObject targetAim = null;
    [SerializeField] private float lookRadius;
    [SerializeField] private string[] commandsAction;
    private Queue<ICommand> SurvivorStates = new Queue<ICommand>();
    private Queue<ICommand> WeaponsStates = new Queue<ICommand>();
    private IEnumerator _survivorStateCoroutine;
    private IEnumerator _weaponsStateCoroutine;
    
    public BaseWeapon Weapon
    {
        get => _weapon;
        set => _weapon = value;
    }

    public float LookRadius => lookRadius;

    private void Awake()
    {
        SurvivorStates.Enqueue(new IdleCommand());
        WeaponsStates.Enqueue(new IdleCommand());
    }

    private void Start()
    {
        Weapon = new BaseWeapon(0.5f, 1f, 2f);
        _survivorStateCoroutine = SurvivorStatesCoroutine();
        _weaponsStateCoroutine = WeaponsStatesCoroutine();
        StartCoroutine(_survivorStateCoroutine);
        StartCoroutine(_weaponsStateCoroutine);
    }

    private IEnumerator WeaponsStatesCoroutine()
    {
        yield break;
    }

    private IEnumerator SurvivorStatesCoroutine()
    {
        while (true)
        {
            CurrentSurvivorCommand = SurvivorStates.Dequeue();
            yield return CurrentSurvivorCommand.Execute();
            if (SurvivorStates.Count == 0) SurvivorStates.Enqueue(CurrentSurvivorCommand);
            yield return null;
        }
    }

    private ZombieBase lastZomby;
    private ICommand CurrentSurvivorCommand;

    public void TakeCommand(string command)
    {
        var tempZombi = user.ChatController.ChekNameZombie(command);
        if (tempZombi)
        {
            
            if (lastZomby != null) lastZomby.LookAtMy(false);
            tempZombi.LookAtMy(true);
           // targetAim = tempZombi.transform;
            lastZomby = tempZombi;
            CurrentSurvivorCommand?.Interrupt();
            SurvivorStates.Enqueue(new AimingCommand(tempZombi.gameObject,this));
        }
        // else
        // {
        //     //send command stack
        //     if (targetAim != null)
        //     {
        //         CommandsStack(command);
        //     }
        // }
        
    }
    

    private void CommandsStack(string content)
    {
        if (commandsAction.Contains(content.ToLower()))
        {
            //add comand
        }
    }


    private void Aiming()
    {
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, LookRadius);
    }
}