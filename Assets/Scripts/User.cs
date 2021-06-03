using UnityEngine;

public class User
{
    public string name;
    public Survivor Character;
    public Color color;
    public ZombieController ZombieController;

    public User(string name, Survivor character, Color color, ZombieController zombieController)
    {
        this.name = name;
        this.color = color;
        this.Character = character;
        this.ZombieController = zombieController;
    }
}