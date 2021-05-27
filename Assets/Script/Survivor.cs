using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script;
using UnityEngine;


public enum StateSurvivor
{
    idle,
    aim,
    reloading
}

public enum StateAction
{
    autoShoot,
    aimShoot,
    shootHead
}


public class Survivor : MonoBehaviour

{
    private BaseWeapon _weapon;
    private StateSurvivor _stateSurvivor;
    public ChatController.User user;
    private Transform targetAim = null;
    [SerializeField] private float lookRadius;
    [SerializeField] private string[] commandsAction;
    public List<Command> _commandsStack = new List<Command>();
    private IEnumerator _commands;

    public StateSurvivor State_Survivor
    {
        get => _stateSurvivor;
        set => _stateSurvivor = value;
    }

    private StateAction State_Action = StateAction.autoShoot;

    public BaseWeapon Weapon
    {
        get => _weapon;
        set => _weapon = value;
    }

    private void Start()
    {
        Weapon = new BaseWeapon(0.5f, 1f, 2f);
    }

    private ZombieBase lastZomby;

    public void TakeCommand(string command)
    {
        var tempZombi = user.ChatController.ChekNameZombie(command);
        if (tempZombi)
        {
            if (lastZomby != null) lastZomby.LookAtMy(false);
            tempZombi.LookAtMy(true);
            targetAim = tempZombi.transform;
            State_Survivor = _stateSurvivor = StateSurvivor.aim;
            _commandsStack.Clear();
            lastZomby = tempZombi;
        }
        else
        {
            //send command stack
            if (targetAim != null)
            {
                CommandsStack(command);
            }
        }
    }

    private void CommandsStack(string content)
    {
        if (commandsAction.Contains(content.ToLower()))
        {
            var commandTemp = new Command(content);
            _commandsStack.Add(commandTemp);
        }
    }

    private void Update()
    {
        SwitctAction();
    }

    private void SwitctAction()
    {
        switch (State_Survivor)
        {
            case StateSurvivor.idle:
                break;
            case StateSurvivor.aim:
                Aiming();
                if (_weapon == null) return;
                if (_commandsStack.Count <= 0) return;
                if (_commandsStack.Count > 0 && _commandsStack == null)
                {
                    // _commands = CommandStack(_weapon);
                }

                break;
            case StateSurvivor.reloading:
                break;
        }
    }

    // private IEnumerator CommandStack(BaseWeapon baseWeapon)
    // {
    //     yield return 
    // }

    private void Aiming()
    {
        if (targetAim != null)
        {
            Vector3 target = targetAim.position;
            target.y = 0;
            Vector3 thisPos = transform.position;
            thisPos.y = 0;
            float distance = Vector3.Distance(target, thisPos);
            if (distance < lookRadius)
            {
                State_Survivor = StateSurvivor.idle;
                targetAim = null;
                lastZomby.LookAtMy(false);
                if (_commandsStack.Count > 0) _commandsStack.Clear();
                return;
            }

            Debug.DrawLine(transform.position, targetAim.position, Color.red);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}