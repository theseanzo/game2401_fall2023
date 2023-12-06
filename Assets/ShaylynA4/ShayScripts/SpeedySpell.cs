using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpeedySpell : MonoBehaviour
{
    public int NewMoveSpeed = 10;
    public ParticleSystem Indicator;
    private bool _affected = false;

    private void OnTriggerEnter(Collider other)
    {
        Unit unit = other.GetComponent<Unit>(); // Reference to the Unit script

        if (unit != null) // If the object is a Unit
        {
            unit.moveSpeed = NewMoveSpeed; // Makes the Units zoomier
            _affected = true;
        }
        else
        {
            return;
        }
       
        if (_affected == true)
        {
            Instantiate(Indicator, unit.transform); // Instantiates the indicator particle effect
            unit.transform.parent = Indicator.transform; // Parents it to the Unit
            
        }

    }
}
