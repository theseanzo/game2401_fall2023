using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBear : Unit
{
    [SerializeField]
    private float ragePowerBoost = 20f; // attack power boost during rage
    [SerializeField]
    private float rageDuration = 5f; // duration of the rage effect
    private bool isRaging = false; // flag to check if the bear is in rage mode
    private int originalAttackPower; // to store the original attack power
    private ParticleSystem attackParticles; // particle system for attack effects

    protected override void Start()
    {
        base.Start();
        originalAttackPower = attackPower; // store the original attack power at start
        attackParticles = GetComponentInChildren<ParticleSystem>(); // get the attack particle system

        // Start the repeating rage activation
        StartCoroutine(RepeatRageActivation());
    }

    private IEnumerator RepeatRageActivation()
    {
        while (true)
        {
            ActivateRage();
            yield return new WaitForSeconds(rageDuration);
            DeactivateRage();
            yield return new WaitForSeconds(3f); // Wait for 3 seconds before repeating
        }
    }

    public void ActivateRage()
    {
        if (!isRaging)
        {
            Debug.Log("Activating Rage");
            isRaging = true;
            attackPower += (int)ragePowerBoost; // increase attack power for rage
            ChangeColorToRageMode(); // change color to indicate rage mode
        }
    }

    private void DeactivateRage()
    {
        Debug.Log("Deactivating Rage");
        isRaging = false;
        attackPower = originalAttackPower; // reset attack power to original
        ResetColor(); // reset color to original
    }


    private void ChangeColorToRageMode()
    {
        Transform targetPartTransform = transform.Find("TeddyWarrior");
        if (targetPartTransform != null)
        {
            Renderer renderer = targetPartTransform.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.red;
            }
        }
    }

    private void ResetColor()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white; // reset to original color
        }
    }
    private void PlayAttackParticles()
    {
        if (attackParticles != null && attackTarget != null)
        {
            attackParticles.transform.position = attackTarget.transform.position; // position particles at the target
            attackParticles.Play(); // play attack particle effect
        }
    }
    private void StopAttackParticles()
    {
        if (attackParticles != null)
        {
            attackParticles.Stop();
        }
    }

    public override void OnAttackActionEvent()
    {
        base.OnAttackActionEvent();

        if (attackTarget != null)
        {
            attackTarget.OnHit(attackPower);

            PlayAttackParticles();
        }
        else
        {
            StopAttackParticles(); 
        }
    }
    
}
