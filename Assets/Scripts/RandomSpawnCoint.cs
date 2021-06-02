using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSpawnCoint : MonoBehaviour
{
    public Action<int, Character> PlayerGetCont;
    [SerializeField] private GameObject coint;
    private List<GameObject> _coints = new List<GameObject>();
    private IEnumerator spawnTimer;
    [SerializeField] private float maxLimitTimeSpawn;
    [SerializeField] private int maxCoint;
    [SerializeField] private int xCount;
    [SerializeField] private int zCount;

    private void Start()
    {
        spawnTimer = SpawnTimer();
        StartCoroutine(spawnTimer);
    }

    private IEnumerator SpawnTimer()
    {
        while (true)
        {

            yield return new WaitForSeconds(maxLimitTimeSpawn);
            if (_coints.Count < maxCoint)
            {
                SpawnCoint();
            }
        }
    }

    private void CoinGeting(GameObject coin, Character character)
    {
        var temp = coin.GetComponent<Coin>();
        PlayerGetCont.Invoke(temp.GetComponent<Coin>().Score,character);
        temp.MeGet -= CoinGeting;
        _coints.Remove(coin);
        Destroy(coin);
    }


    public void SpawnCoint()
    {
        var coint = Instantiate(this.coint);
        coint.transform.position = new Vector3(Random.Range(1, xCount), 1, Random.Range(1, zCount));
        coint.GetComponent<Coin>().MeGet += CoinGeting;
        _coints.Add(coint);
    }
}