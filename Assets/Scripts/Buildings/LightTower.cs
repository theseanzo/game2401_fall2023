using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTower : Building
{
    [SerializeField]
    GameObject targetRange;
    public List<GameObject> unitsInRange = new List<GameObject>();
    
    public ParticleSystem protectionEffect; // reference to the particle system for visual effects
    public ParticleSystem defenseExplosion;

    [SerializeField] private Color normalColor = Color.white; // color when building is not damaged
    [SerializeField] private Color damagedColor = Color.red; // color when building is damaged
    [SerializeField] private Color repairingColor = Color.yellow; // color when building is repairing

    private MeshRenderer meshRenderer; // mesh renderer for changing the building's color
    public int maxHealth = 120;
    public int currentHealth;
    private float damageEffectDuration = 2f;
    public int halfHealth;
    private bool abilityActivated = false;

    protected override void Start()
    {
        base.Start();
        currentHealth = maxHealth; // set current health to max at start
        meshRenderer = GetComponentInChildren<MeshRenderer>(); // get the mesh renderer component
        UpdateVisuals();
        halfHealth = maxHealth / 2;
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

        if (currentHealth <= halfHealth && abilityActivated == false)
        {
            Ability();
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Unit")
        {
            unitsInRange.Add(other.gameObject);
        }
    }

    private void Ability()
    {
        abilityActivated = true;
        currentHealth += 10;
        if(defenseExplosion != null)
        {
            defenseExplosion.Play();
        }
        else
        {
            Debug.Log("Particle effects not found");
        }
        for(int i = 0; i < unitsInRange.Count; i++)
        {
            Destroy(unitsInRange[i]);
        }
        
    }
}


