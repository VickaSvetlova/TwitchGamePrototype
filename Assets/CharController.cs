using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharController : MonoBehaviour
{
    [SerializeField] private GameObject prefabPlayer;
    [SerializeField] private Transform[] PositionSpawn;
    private List<Transform> _positionSpawn = new List<Transform>();
    public GameManager Manager { get; set; }

    private void Awake()
    {
        ResetPositionArray();
    }

    public User CreateCharacter(string chatName)
    {
        var player = Instantiate(prefabPlayer);
        player.transform.position = GetRandomSpawn();
        var temp = player.GetComponent<Survivor>();
        var color = GetRandomColor();
        var user = new User(chatName, temp, color, Manager.ZombieController);
        temp.user = user;
        player.GetComponent<Renderer>().material.color = color;
        return user;
    }

    private Vector3 GetRandomSpawn()
    {
        if (_positionSpawn.Count > 0)
        {
            var randomPosition = Random.Range(0, _positionSpawn.Count);
            var thisPosition = _positionSpawn[randomPosition].position;
            _positionSpawn.RemoveAt(randomPosition);
            return thisPosition;
        }
        else
        {
            ResetPositionArray();
            GetRandomSpawn();
        }

        return Vector3.zero;
    }

    private void ResetPositionArray()
    {
        _positionSpawn.AddRange(PositionSpawn);
    }


    private Color GetRandomColor()
    {
        return Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }
}