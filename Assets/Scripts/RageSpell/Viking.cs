using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viking : Unit
{
    [SerializeField]
    private float speedBoost = 2f; // Speed increase factor when speeding
    [SerializeField]
    private float speedDuration = 5f; // Duration of the increased speed state
    private bool isSpeeding = false; // Tracks if Pekka is in speed mode
    private float baseRunSpeed; // To store Pekka's base run speed
    private ParticleSystem speedEffectParticles; // Particle system for speed effects

    // Add a runSpeed property
    public float runSpeed { get; private set; }

    protected override void Start()
    {
        base.Start();
        baseRunSpeed = runSpeed; // Store the base run speed
        speedEffectParticles = GetComponentInChildren<ParticleSystem>(); // Get the speed effect particle system

        // Initialize runSpeed here or somewhere appropriate
        runSpeed = 5f; // Example initial speed

        // Start the repeating speed activation cycle
        StartCoroutine(SpeedCycle());
    }

    private IEnumerator SpeedCycle()
    {
        while (true)
        {
            EnterSpeedState();
            yield return new WaitForSeconds(speedDuration);
            ExitSpeedState();
            yield return new WaitForSeconds(3f); // Cooldown before next speed burst
        }
    }

    public void EnterSpeedState()
    {
        if (!isSpeeding)
        {
            Debug.Log("Pekka is now speeding!");
            isSpeeding = true;
            runSpeed *= speedBoost; // Increase run speed in speed mode
            ApplySpeedAppearance(); // Change appearance for speed mode
        }
    }

    private void ExitSpeedState()
    {
        Debug.Log("Pekka's speed burst has ended.");
        isSpeeding = false;
        runSpeed = baseRunSpeed; // Revert run speed
        RevertAppearance(); // Return to normal appearance
    }

    private void ApplySpeedAppearance()
    {
        Transform targetTransform = transform.Find("PekkaModel");
        if (targetTransform != null)
        {
            Renderer renderer = targetTransform.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.blue; // Speed color
            }
        }
    }

    private void RevertAppearance()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white; // Normal color
        }
    }

    private void TriggerSpeedEffects()
    {
        if (speedEffectParticles != null && attackTarget != null)
        {
            speedEffectParticles.transform.position = attackTarget.transform.position; // Position effects at the target
            speedEffectParticles.Play(); // Play speed effect
        }
    }

    private void StopSpeedEffects()
    {
        if (speedEffectParticles != null)
        {
            speedEffectParticles.Stop();
        }
    }

    public override void OnAttackActionEvent()
    {
        base.OnAttackActionEvent();

        if (attackTarget != null)
        {
            attackTarget.OnHit(attackPower);

            TriggerSpeedEffects();
        }
        else
        {
            StopSpeedEffects();
        }
    }
    
}
