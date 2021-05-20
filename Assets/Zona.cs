using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zona : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        var temp = other.GetComponent<Character>();

        if (temp != null)
        {
            temp.GetCoin(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var temp = other.GetComponent<Character>();

        if (temp != null)
        {
            temp.GetCoin(false);
        }
    }
}
