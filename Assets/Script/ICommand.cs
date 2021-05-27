using System.Collections;
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
        yield break;
    }

    public void Interrupt()
    {
        
    }
}

public class IdleCommandFactory : CommandFactory
{
    public override bool Match(string textCommand)
    {
        return false;
    }

    public override ICommand CreateCommand(Survivor player)
    {
        return new IdleCommand();
    }
}

public class AimingCommand : ICommand
{
    public GameObject TargetAim;
    public Survivor Player { get; set; }

    public IEnumerator Execute()
    {
        while (TargetAim != null)
        {
            Aim();
            yield return null;
           
        }
    }

    public void Interrupt()
    {
        TargetAim = null;
    }

    private void Aim()
    {
        Vector3 target = TargetAim.transform.position;
        target.y = 0;
        Vector3 thisPos = Player.transform.position;
        thisPos.y = 0;
        float distance = Vector3.Distance(target, thisPos);
        if (distance < Player.LookRadius)
        {
            TargetAim = null;
            return;
        }

        Debug.DrawLine(Player.transform.position, TargetAim.transform.position, Color.red);
    }

    public AimingCommand(GameObject TargetAim, Survivor player)
    {
        this.TargetAim = TargetAim;
        this.Player = player;
    }
}