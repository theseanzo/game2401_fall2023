using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chef : Unit
{
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {

    }
    //let's override the OnAttackActionEvent and do damage to our target
    public override void OnAttackActionEvent()
    {
        base.OnAttackActionEvent(); //first let's do the parent's OnAttackActionEvent (just in case)
        if (attackTarget != null)//make sure the target has not been destroyed
        {
            attackTarget.OnHit(attackPower);
        }
    }
}
