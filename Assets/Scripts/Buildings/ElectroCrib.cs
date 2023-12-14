using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroCrib : Building
{
    [SerializeField] private float attackPower = 100;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float attackInterval = 1f;
    [SerializeField] private Transform fireBallPos;
    [SerializeField] GameObject damageParticle;
    [SerializeField] ParticleSystem hitParticle;
    Unit attackTarget;

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
        Vector3 offset = new Vector3(0, 1, 0);
        Quaternion rotation = Quaternion.Euler(90f,0, 0);

        GameObject effect = Instantiate(damageParticle, unit.transform.position + offset , rotation);

        effect.transform.parent = unit.transform;
    }

    private void AttackTarget()
    {
        if (attackTarget != null)
        {
            PoolObject fireball = PoolManager.Instance.Spawn("FireBall");
            fireball.transform.position = fireBallPos.position;
            fireball.transform.rotation = fireBallPos.rotation;
            fireball.GetComponent<Projectile>().Init(attackTarget, (int)attackPower);
        }
    }
    public override void OnHit(int damage)
    {
        base.OnHit(damage);
        // Additional functionality specific to ElectroCrib's OnHit if needed
        hitParticle.Play();
    }
    private void OnDrawGizmos()
     {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
     }
}
