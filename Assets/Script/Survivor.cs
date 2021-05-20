using System;
using Script;
using UnityEngine;


public enum StateSurvivor
{
    idle,
    aim,
    autoShoot,
    aimShoot,
    shootHead,
    reloading
}


public class Survivor : MonoBehaviour

{
    [SerializeField] private string[] commands;
    private StateSurvivor state;
    public ChatController.User user;
    private Transform targetAim = null;
    [SerializeField] private float lookRadius;

    public StateSurvivor State
    {
        get => state;
        set => state = value;
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
            State = state = StateSurvivor.aim;
            lastZomby = tempZombi;
        }
    }

    private void Update()
    {
        switch (State)
        {
            case StateSurvivor.idle:
                break;
            case StateSurvivor.aim:
                Aiming();
                break;
            case StateSurvivor.autoShoot:
                break;
            case StateSurvivor.aimShoot:
                break;
            case StateSurvivor.shootHead:
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
                State = StateSurvivor.idle;
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