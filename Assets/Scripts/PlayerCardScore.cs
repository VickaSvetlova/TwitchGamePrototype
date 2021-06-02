using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardScore : MonoBehaviour
{
    [SerializeField] private Text name;
    [SerializeField] private Text score;
    [SerializeField] private Image renderer;

    public void SetScore(string name, int score, Color valueColor)
    {
        this.name.text = name;
        this.score.text = score.ToString();
        this.renderer.color = valueColor;
    }
}