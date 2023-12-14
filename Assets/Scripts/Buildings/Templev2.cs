using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Templev2 : Building
{
    [SerializeField] private ParticleSystem damageEffect;
    public override void OnHit(int damage)
    {
        base.OnHit(damage);
        
        damageEffect.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
