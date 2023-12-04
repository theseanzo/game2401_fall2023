using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlagueDoctor : Unit
{
    // Update is called once per frame
    void Update()
    {

    }

    public override void OnAttackActionEvent()
    {
        base.OnAttackActionEvent(); //first let's do the parent's OnAttackActionEvent (just in case)
        if (attackTarget != null)//make sure the target has not been destroyed
        {
            attackTarget.OnHit(attackPower);
        }
    }

}