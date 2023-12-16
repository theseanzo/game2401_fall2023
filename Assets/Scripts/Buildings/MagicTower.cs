using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTower : Building
{
    public float freezeInterval = 2f; // Frequency of freezes when in the collider
    public float freezeDuration = 0.5f; // Duration to freeze units in seconds
    public SphereCollider magicCollider; 
    public ParticleSystem spellVFX;
    public AudioSource spellSFX;

    private List<Unit> affectedUnits = new List<Unit>(); // List to store affected units
    private float originalSpeed;

    void Start()
    {
        // Ensure the collider is set to trigger mode
        if (magicCollider != null)
        {
            magicCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit unit = other.GetComponent<Unit>();
        if (unit != null)
        {
            spellVFX.Play();
            spellSFX.Play();
            SlowDownMovement(unit);
            affectedUnits.Add(unit);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Unit unit = other.GetComponent<Unit>();
        if (unit != null && !affectedUnits.Contains(unit))
        {
            spellVFX.Stop();
            spellSFX.Stop();
            RestoreSpeed(unit);
            affectedUnits.Remove(unit);
        }
    }

    private void OnDestroy()
    {
        foreach (Unit unit in affectedUnits)
        {
            RestoreSpeed(unit);
        }

        // Clear the list of affected units
        affectedUnits.Clear();
    }

    // Function to freeze unit movement
    private void SlowDownMovement(Unit unit)
    {
        originalSpeed = unit.moveSpeed;

        // Stop the movement of the unit
        unit.moveSpeed /= 2;
    }

    // Function to restore unit speed
    private void RestoreSpeed(Unit unit)
    {
        if (unit != null)
        {
            unit.moveSpeed = originalSpeed;
        }
    }
}