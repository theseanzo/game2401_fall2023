using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chef : Unit
{

    [SerializeField]
    Transform fireStartPos;
    //the chef will be base on the archer since I was thinking of the chef making fire because he can cook, but can't figure how to make him do a ring of fire so I settle with throwing a fireball
    public override void OnAttackActionEvent()
    {
        base.OnAttackActionEvent();
        if (attackTarget != null)
        {
            PoolObject fire = PoolManager.Instance.Spawn("Fire"); //get a fireball
            fire.transform.position = fireStartPos.position;
            fire.transform.rotation = fireStartPos.rotation;
            fire.GetComponent<Projectile>().Init(attackTarget, attackPower);
        }
    }
}
