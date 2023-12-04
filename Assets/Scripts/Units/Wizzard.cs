using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizzard : Unit
{
    [SerializeField]
    Transform spellBallStartPos;
    // Start is called before the first frame update
    public override void OnAttackActionEvent()
    {
        base.OnAttackActionEvent();
        if (attackTarget != null)
        {
            PoolObject spellball = PoolManager.Instance.Spawn("SpellBall"); //grab an arrow
            spellball.transform.position = spellBallStartPos.position;
            spellball.transform.rotation = spellBallStartPos.rotation; //set up our arrow to follow the arrowStartPos and rotation
            spellball.GetComponent<Projectile>().Init(attackTarget, attackPower);
        }
    }
}
