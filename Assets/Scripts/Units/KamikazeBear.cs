using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KamikazeBear : Unit
{

    public UnityEvent OnExplosion;

    protected override void Start()
    {
        base.Start();
    }

    public override void OnAttackActionEvent()
    {
        base.OnAttackActionEvent(); //first let's do the parent's OnAttackActionEvent (just in case)
        if (attackTarget != null)//make sure the target has not been destroyed
        {
            StopAllCoroutines();
            attackTarget.OnHit(attackPower);
            OnExplosion.Invoke();
        }
    }
    
}
