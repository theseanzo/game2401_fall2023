using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catboy : Unit
{
    public GameObject Cat;

    public override void OnAttackActionEvent() // Overriding the parent's function
    {
        base.OnAttackActionEvent(); // Completes the parent's OnAttackActionEvent
        if (attackTarget != null) // Checks to make sure there is a target
        {
            attackTarget.OnHit(attackPower);

            Instantiate(Cat, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
