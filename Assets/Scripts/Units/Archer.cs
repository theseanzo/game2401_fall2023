using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit
{

    [SerializeField]
    Transform arrowStartPos;
    // Start is called before the first frame update
    public override void OnAttackActionEvent()
    {
        base.OnAttackActionEvent();
        if(attackTarget != null)
        {
            PoolObject arrow = PoolManager.Instance.Spawn("Arrow"); //grab an arrow
            arrow.transform.position = arrowStartPos.position;
            arrow.transform.rotation = arrowStartPos.rotation; //set up our arrow to follow the arrowStartPos and rotation
            arrow.GetComponent<Projectile>().Init(attackTarget, attackPower);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
