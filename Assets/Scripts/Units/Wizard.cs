using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Unit
{

    [SerializeField]
    Transform fireBallPos;
    // Start is called before the first frame update
    public override void OnAttackActionEvent()
    {
        base.OnAttackActionEvent();
        if (attackTarget != null)
        {
            PoolObject fireball = PoolManager.Instance.Spawn("FireBall"); 
            fireball.transform.position = fireBallPos.position;
            fireball.transform.rotation = fireBallPos.rotation; 
            fireball.GetComponent<Projectile>().Init(attackTarget, attackPower);
        }
    }
}
