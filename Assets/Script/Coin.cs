using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public enum TypeCoin
{
cooper=0, silver=1, gold=2
}
public class Coin : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    public Action<GameObject,Character> MeGet;
    public int Score=1;
    public TypeCoin typecoin;
    public GameObject[] coinGR;

    public void SetCoin(TypeCoin typeCoin)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherSome = other.GetComponent<Character>();
        if (otherSome)
        {
            var temp= Instantiate(_particleSystem,transform.position,Quaternion.identity);
            temp.Emit(1);
            MeGet?.Invoke(gameObject,otherSome);
            
        }
    }
}
