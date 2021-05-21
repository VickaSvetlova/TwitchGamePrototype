using System;
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
    [SerializeField] private string[] commands;
    private StateSurvivor _stateSurvivor;
    public ChatController.User user;
    private Transform targetAim = null;
    [SerializeField] private float lookRadius;

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
            lastZomby = tempZombi;
        }
    }

    private void Update()
    {
        switch (State_Survivor)
        {
            case StateSurvivor.idle:
                break;
            case StateSurvivor.aim:
                Aiming();
                switch (State_Action)
                {
                    case StateAction.autoShoot:
                        _weapon.Shoot(TimeAiming.auto);
                        break;
                    case StateAction.aimShoot:
                        _weapon.Shoot(TimeAiming.aiming);
                        break;
                    case StateAction.shootHead:
                        _weapon.Shoot(TimeAiming.headShoot);
                        break;
                }

                break;
            case StateSurvivor.reloading:
                break;
        }
    }

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