using System;

[Serializable]
public class Command
{
    public TimeAiming command;

    public Command(string content)
    {
        SetComand(content);
    }

    public void SetComand(string content)
    {
        var temp=TimeAiming.auto.ToString();
        if (content.Contains(TimeAiming.auto.ToString().ToLower()))
        {
            command = TimeAiming.auto;
            return;
        }

        if (content.Contains(TimeAiming.aim.ToString().ToLower()))
        {
            command = TimeAiming.aim;
            return;
        }

        if (content.Contains(TimeAiming.headShoot.ToString().ToLower()))
        {
            command = TimeAiming.headShoot;
        }
    }
}