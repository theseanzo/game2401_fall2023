using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpeedySpell : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.GetComponent<Unit>())
        {

        }
    }
}
