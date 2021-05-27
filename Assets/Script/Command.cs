using System;

[Serializable]
public class Command
{
    public string command;

    public Command(string content)
    {
        command = content;
    }
}