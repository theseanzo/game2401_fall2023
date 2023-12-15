using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defence : MonoBehaviour
{
    public float NewMoveSpeed = 0.5f;
    public float NewAttackInterval = 3.5f;

    public ParticleSystem Indicator;

    public ParticleSystem Cone;

    private bool _affected = false; // Whether or not the Units are affected by the building

    void Awake()
    {
        Cone = GetComponentInChildren<ParticleSystem>(); // Finds the Particle System

    }

    private void OnTriggerEnter(Collider other)
    {
        Unit unit = other.GetComponent<Unit>(); // Reference to the Unit script

        if (unit != null) // If the object is a Unit
        {
            _affected = true;
            Cone?.Play(); // Plays the Particle System on the building (as long as there is one)
            transform.GetChild(0)?.gameObject.SetActive(true); // Sets the child of this object (the empty object holding the cone carousel) active (as long as it exists)      

            // The spinning cones have made the Unit dizzy!
            unit.moveSpeed = NewMoveSpeed; // He's struggling to move!
            unit.attackInterval = NewAttackInterval; // He can't focus on his attacks!
        }
        else
        {
            return;
        }

        if (_affected == true)
        {
            Instantiate(Indicator, unit.transform); // Instantiates the dizzy particle effect
            unit.transform.parent = Indicator.transform; // Parents it to the Unit
        }

    }
}
