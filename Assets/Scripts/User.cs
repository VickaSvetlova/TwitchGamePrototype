using UnityEngine;

public class User
{
    public string Name { get; }
    public Survivor Character { get; }
    public Color Color { get; }
    public IZombieProvider ZombieProvider { get; }

    public User(string name, Survivor character, Color color, IZombieProvider zombieProvider)
    {
        Name = name;
        Color = color;
        Character = character;
        ZombieProvider = zombieProvider;
    }
}