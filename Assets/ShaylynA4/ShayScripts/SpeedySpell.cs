using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpeedySpell : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Building building = other.GetComponent<Building>();

        if (building != null) // If the object has the Building script
        {
            Destroy(other.gameObject); // This is just a test
        }
    }
}

// This works with the buildings, but not with any Units, even if I specify a certain type of Unit (like Soldier.) I also don't know how I'm supposed to modify the stats of the Unit since the setter is protected.
// It works regardless of if I'm in Build or Attack mode (with the buildings.) The buildings come back after loading into a different mode (which is what I want, since I don't want the spell to last forever.)
