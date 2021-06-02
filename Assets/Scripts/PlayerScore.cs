using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private GameObject tableScore;
    [SerializeField] private GameObject playerTableScore;
    public List<KeyValuePair<string, ChatController.User>> sorted=new List<KeyValuePair<string, ChatController.User>>();

    public void SetTableScore(Dictionary<string, ChatController.User> players)
    {

        sorted = players.ToList();
      //  sorted.Sort((x,y)=>-x.Value.Character.coin.CompareTo(y.Value.Character.coin));

        var child = tableScore.transform.childCount;
        for (int i = 0; i < child; i++)
        {
            Destroy(tableScore.transform.GetChild(i).gameObject);
        }

        foreach (var element in sorted)
        {
            var temp = Instantiate(playerTableScore);
          //  temp.GetComponent<PlayerCardScore>().SetScore(element.Key,element.Value.Character.coin,element.Value.color);
          //  temp.GetComponent<PlayerCardScore>().SetScore(element.Key, element.Value.player.coin);
            temp.transform.SetParent(tableScore.transform);
        }
    }
}