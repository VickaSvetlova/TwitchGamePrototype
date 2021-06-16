using UnityEngine;

public class User
{
    public string NameUser { get; }
    public Survivor Character { get; }
    public Color Color { get; }
    public IZombieProvider ZombieProvider { get; }

    public User(string nameUser, Survivor character, Color color, IZombieProvider zombieProvider)
    {
        NameUser = nameUser;
        Color = color;
        Character = character;
        ZombieProvider = zombieProvider;
    }
}