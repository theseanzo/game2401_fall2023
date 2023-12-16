using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cryomancer : Unit
{

    [SerializeField]
    Transform iceSpellStartPos;
    // Start is called before the first frame update
    public override void OnAttackActionEvent()
    {
        base.OnAttackActionEvent();
        if (attackTarget != null)
        {
            PoolObject iceSpell = PoolManager.Instance.Spawn("IceSpell"); //cast an icespell
            iceSpell.transform.position = iceSpellStartPos.position;
            iceSpell.transform.rotation = iceSpellStartPos.rotation; //set up our iceSpell to follow the iceSpellStartPos and rotation
            iceSpell.GetComponent<Projectile>().Init(attackTarget, attackPower);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
