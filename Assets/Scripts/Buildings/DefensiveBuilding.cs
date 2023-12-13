using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveBuilding : Building
{
    public ParticleSystem protectionEffect; // reference to the particle system for visual effects

    [SerializeField] private Color normalColor = Color.white; // color when building is not damaged
    [SerializeField] private Color damagedColor = Color.red; // color when building is damaged
    [SerializeField] private Color repairingColor = Color.yellow; // color when building is repairing

    private MeshRenderer meshRenderer; // mesh renderer for changing the building's color
    public int maxHealth = 100; 
    private int currentHealth;
    private float damageEffectDuration = 2f; 

    protected override void Start()
    {
        base.Start();
        currentHealth = maxHealth; // set current health to max at start
        meshRenderer = GetComponentInChildren<MeshRenderer>(); // get the mesh renderer component
        UpdateVisuals();
    }

    public override void OnHit(int damage)
    {
        base.OnHit(damage);
        currentHealth -= damage; // reduce health when hit
        StartCoroutine(ShowDamageEffect()); // start showing the damage effect

        TriggerProtectionEffect(); // trigger the protection particle effect
    }

    private IEnumerator ShowDamageEffect()
    {
        if (meshRenderer != null)
        {
            meshRenderer.material.color = damagedColor; // change color to damaged color
            yield return new WaitForSeconds(damageEffectDuration); // wait for the duration
            meshRenderer.material.color = repairingColor; // change color to repairing color
        }
    }

    protected void Update()
    {
        if (currentHealth < maxHealth)
        {
            RepairBuilding(); // repair the building if health is not full
        }
    }

    private void RepairBuilding()
    {
        currentHealth = Mathf.Min(currentHealth + 1, maxHealth); // increase health
        UpdateVisuals(); 
    }

    private void UpdateVisuals()
    {
        if (meshRenderer != null)
        {
            // change color based on whether the building is repairing or not
            meshRenderer.material.color = currentHealth < maxHealth ? repairingColor : normalColor;
        }
    }

    public override void OnDie()
    {
        base.OnDie(); // call the base class on die method
        TriggerProtectionEffect(); // trigger protection effect when the building dies
    }

    private void TriggerProtectionEffect()
    {
        if (protectionEffect != null)
        {
            protectionEffect.Play(); // play the particle effect
        }
        else
        {
            Debug.Log("Particle effect not found"); 
        }
    }
}

