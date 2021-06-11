using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    IEnumerator Execute();
    void Interrupt();
}

public abstract class CommandFactory
{
    public abstract bool Match(string textCommand);
    public abstract ICommand CreateCommand(Survivor player);
}

public class IdleCommand : ICommand
{
    public IEnumerator Execute()
    {
        Debug.Log("Idle");
        yield break;
    }

    public void Interrupt()
    {
    }
}

/// <summary>
/// /////////////////////////////////////////////
/// </summary>
/// 
public class AimingCommand : ICommand
{
    public GameObject TargetAim;
    public Survivor Player { get; set; }

    public IEnumerator Execute()
    {
        while (TargetAim != null)
        {
            Aim();
            Debug.Log("aiming");
            yield return null;
        }
    }

    public void Interrupt()
    {
        TargetAim = null;
    }

    private void Aim()
    {
        if (Extension.DistanceCalculate(Player.gameObject, TargetAim) < Player.LookRadius)
        {
            TargetAim = null;
            return;
        }

        Debug.DrawLine(Player.transform.position, TargetAim.transform.position, Color.green);
    }

    public AimingCommand(GameObject TargetAim, Survivor player)
    {
        this.TargetAim = TargetAim;
        this.Player = player;
    }
}

public class AutoShootCommand : ICommand
{
    private Survivor Player { get; set; }
    private GameObject TargetAim { get; set; }

    protected float coolDown { get; set; }

    public IEnumerator Execute()
    {
        if (TargetAim != null)
        {
            if (Extension.DistanceCalculate(Player.gameObject, TargetAim) < Player.LookRadius)
            {
                TargetAim = null;
                yield break;
            }

            yield return new WaitForSeconds(coolDown);
            DebugLog();
            yield return Player.Weapon.Shoot();
        }

       
    }

    public virtual void DebugLog()
    {
        Debug.Log("pulling the trigger AutoShoot");
    }

    public void Interrupt()
    {
        TargetAim = null;
    }


    public AutoShootCommand(GameObject targetAim, Survivor player)
    {
        this.TargetAim = targetAim;
        this.Player = player;
        this.coolDown = player.Weapon.auto;
    }
}

/// <summary>
/// //////////////
/// </summary>
public class AimedShotCommand : AutoShootCommand
{
    public AimedShotCommand(GameObject targetAim, Survivor player) : base(targetAim, player)
    {
        this.coolDown = player.Weapon.aiming;
    }

    public override void DebugLog()
    {
        Debug.Log("pulling the trigger AimedShoot");
    }
}

public class AimedShotFactory : CommandFactory
{
    public override bool Match(string textCommand)
    {
        return "aim" == textCommand;
    }

    public override ICommand CreateCommand(Survivor player)
    {
        return new AimedShotCommand(player.TargetAim, player);
    }
}

/// <summary>
/// /////////
/// </summary>
public class HeadshotCommand : AutoShootCommand
{
    public HeadshotCommand(GameObject targetAim, Survivor player) : base(targetAim, player)
    {
        this.coolDown = player.Weapon.headShoot;
    }
    public override void DebugLog()
    {
        Debug.Log("pulling the trigger HeadShoot");
    }
}

public class HeadshotShotFactory : CommandFactory
{
    public override bool Match(string textCommand)
    {
        return "headshot" == textCommand;
    }

    public override ICommand CreateCommand(Survivor player)
    {
        return new HeadshotCommand(player.TargetAim, player);
    }
}
/// <summary>
/// /////
/// </summary>
public static class Extension
{
    public static float DistanceCalculate(GameObject player, GameObject targetAim)
    {
        Vector3 target = targetAim.transform.position;
        target.y = 0;
        Vector3 thisPos = player.transform.position;
        thisPos.y = 0;
        float distance = Vector3.Distance(target, thisPos);

        return distance;
    }
}