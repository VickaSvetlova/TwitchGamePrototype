using System;
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

    public StateSurvivor State
    {
        get => state;
        set => state = value;
    }

    public void TakeCommand(string command)
    {
        var tempZombi = user.ChatController.ChekNameZombie(command);
        if (tempZombi)
        {
            targetAim = tempZombi.transform;
            State = state = StateSurvivor.aim;
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
        Debug.DrawLine(transform.position, targetAim.position);
    }
}