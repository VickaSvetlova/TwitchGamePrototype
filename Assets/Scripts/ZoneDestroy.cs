using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class ZoneDestroy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var temp = other.GetComponent<ZombieBase>();
        if (temp != null)
        {
            temp.Goal();
        }
    }
}