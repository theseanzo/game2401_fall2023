using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giant : Unit
{
    [SerializeField]
    Transform startPos;
    [SerializeField] private float _startingAttackInterval = 3f;
    private float _endingAttackInterval;
    [SerializeField] private Transform _projectileModel;

    private Building _consistentTarget;
    public override void OnAttackActionEvent()
    {
        base.OnAttackActionEvent();
        if (attackTarget != null)
        {
            _projectileModel?.gameObject.SetActive(false);
            PoolObject bear = PoolManager.Instance.Spawn("ThrownBear"); //gets bear and throws it
            bear.transform.position = startPos.position;
            bear.transform.rotation = startPos.rotation; 
            bear.GetComponent<SpawningProjectile>().Init(attackTarget, attackPower);
            if(attackTarget != _consistentTarget)//sets the consistent target to its current target to slow its next attacks on that building
            {
                _consistentTarget = attackTarget;
            }
        }
    }

    private void Awake()
    {
        
        _projectileModel?.gameObject.SetActive(false);
        _endingAttackInterval = attackInterval;
        attackInterval = _startingAttackInterval;
    }
    void Update() //giant should make its first attack faster than its follow up attacks on each building
    {
        if(attackTarget != _consistentTarget && attackTarget != null)
        {
            attackInterval = _startingAttackInterval;
        }
        else
        {
            attackInterval = _endingAttackInterval;
        }
    }

    protected override void Attack()
    {
        base.Attack();
        _projectileModel?.gameObject.SetActive(true);
    }
}
