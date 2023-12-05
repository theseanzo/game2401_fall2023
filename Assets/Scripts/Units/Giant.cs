using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giant : Unit
{
    [SerializeField]
    Transform arrowStartPos;
    public override void OnAttackActionEvent()
    {
        base.OnAttackActionEvent();
        if (attackTarget != null)
        {
            PoolObject arrow = PoolManager.Instance.Spawn("ThrownBear"); //grab an arrow
            arrow.transform.position = arrowStartPos.position;
            arrow.transform.rotation = arrowStartPos.rotation; //set up our arrow to follow the arrowStartPos and rotation
            arrow.GetComponent<SpawningProjectile>().Init(attackTarget, attackPower);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
