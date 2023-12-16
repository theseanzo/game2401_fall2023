using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentEffect : MonoBehaviour
{
    public float DebuffSpeed = 0.5f;
    public ParticleSystem GhostEffect;
    public ParticleSystem BloodParticle;
    private bool _slowed = false;

    void Awake() //Find Particle System on awake
    {
        BloodParticle = GetComponent<ParticleSystem>(); //Find the Particle System in building
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit unit = other.GetComponent<Unit>(); //Reference the Unit script
        if (unit != null)
        {
            BloodParticle.Play(); //Play Particle System once Unit enter trigger box
            _slowed = true;
            unit.moveSpeed = DebuffSpeed;
            //The bears get stab by the tent so they will move slower, it's also permanent
        }

        if (_slowed == true)
        {
            Instantiate(GhostEffect, unit.transform); //Instantiate the particle system
            unit.transform.parent = GhostEffect.transform; //Parent the ghost effect onto the unit
        }
    }
}
