using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defence : MonoBehaviour
{
    public int NewMoveSpeed = 10;
    public ParticleSystem Indicator;
    public ParticleSystem Cone;
    private bool _affected = false;

    void Awake()
    {
        Cone = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit unit = other.GetComponent<Unit>(); // Reference to the Unit script

        if (unit != null) // If the object is a Unit
        {
            _affected = true;
            Cone.Play();
            
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
