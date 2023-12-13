using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroCrib : Building
{
    [SerializeField]
    private int attackPower = 100;
    [SerializeField]
    private float attackRange = 10f;
    [SerializeField]
    private float attackInterval = 1f;
    [SerializeField]
    private Transform fireBallPos;

    Unit attackTarget;
    GameObject damageParticle;

    float time;
    private void Awake()
    {
        time = attackInterval;
    }
    private void Update()
    {
        time += Time.deltaTime;
        if (time > attackInterval)
        {
            time = 0;
            Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
            FindAndDamageUnit(colliders);
            AttackTarget();
        }
    }

    private void FindAndDamageUnit(Collider[] colliders)
    {
        foreach (Collider collider in colliders)
        {
            Unit unit = collider.gameObject.GetComponent<Unit>();
            if (unit != null && attackTarget == null)
            {
                attackTarget = unit;
                CreateDamageEffect(unit);
            }
        }
    }

    private void CreateDamageEffect(Unit unit)
    {
       // GameObject effect = Instantiate(damageParticle, unit.transform.position, Quaternion.identity);
       // effect.transform.parent = unit.transform;
    }

    private void AttackTarget()
    {
        if (attackTarget != null)
        {
            PoolObject fireball = PoolManager.Instance.Spawn("FireBall");
            fireball.transform.position = fireBallPos.position;
            fireball.transform.rotation = fireBallPos.rotation;
            fireball.GetComponent<Projectile>().Init(attackTarget, attackPower);
        }
    }
     private void OnDrawGizmos()
     {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
     }
}
