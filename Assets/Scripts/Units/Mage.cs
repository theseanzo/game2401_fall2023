using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Unit
{
    [SerializeField]
    Transform magicStartPos;
    // Start is called before the first frame update
    public override void OnAttackActionEvent()
    {
        base.OnAttackActionEvent();  // Call the base method to trigger the base class coroutine
        Debug.Log("Mage OnAttackActionEvent");
        if (attackTarget != null)
        {
            PoolObject magicSpell = PoolManager.Instance.Spawn("MagicSpell");
            if (magicSpell != null)
            {
                Debug.Log("Spawning magic spell");
                magicSpell.transform.position = magicStartPos.position;
                magicSpell.transform.rotation = magicStartPos.rotation;
                magicSpell.GetComponent<Projectile>().Init(attackTarget, attackPower);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
