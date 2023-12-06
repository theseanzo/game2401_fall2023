using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpeedySpell : MonoBehaviour
{
    public int NewMoveSpeed = 10;

    private void OnTriggerEnter(Collider other)
    {
        Unit unit = other.GetComponent<Unit>(); // Reference to the Unit script

        if (unit != null) // If the object is a Unit
        {
            unit.moveSpeed = NewMoveSpeed; // Makes the Units zoomier    
        }
        else
        {
            return;
        }
    }
}
