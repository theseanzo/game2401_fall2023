using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catboy : Unit
{
    public override void OnAttackActionEvent() // Overriding the parent's function
    {
        base.OnAttackActionEvent(); // Completes the parent's OnAttackActionEvent
        if (attackTarget != null)
        {
            attackTarget.OnHit(attackPower);
        }
    }
}
