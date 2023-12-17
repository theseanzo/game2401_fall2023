using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerzCastle : Building
{
    [Header("Arrow Settings")]
    [SerializeField] private Transform[] arrowStartPositions;
    [SerializeField] private int arrowPower;
    [SerializeField] private float attackInterval;

    [Header("Spell Settings")]
    [SerializeField] private float spellRadius;
    [SerializeField] private float freezeDuration = 0.5f;
    [SerializeField] private ParticleSystem spellVFX;

    private List<BaseObject> attackTargets = new List<BaseObject>();
    private List<Unit> affectedUnits = new List<Unit>();

    protected override void Start()
    {
        base.Start();
        StartCoroutine(ContinuousAttack());
        StartCoroutine(SpellEffect());
    }

    private void Update()
    {
        DetectAndTrackTargets();
    }

    private void DetectAndTrackTargets()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, spellRadius);
        foreach (var hitCollider in hitColliders)
        {
            BaseObject target = hitCollider.gameObject.GetComponent<BaseObject>();
            if (target != null && !attackTargets.Contains(target))
            {
                attackTargets.Add(target);
                FireAtTarget(target);
            }
        }
    }

    private void FireAtTarget(BaseObject target)
    {
        foreach (var arrowStartPos in arrowStartPositions)
        {
            FireArrow(target, arrowStartPos);
        }
    }

    IEnumerator ContinuousAttack()
    {
        while (true)
        {
            foreach (BaseObject target in attackTargets)
            {
                FireAtTarget(target);
            }

            yield return new WaitForSeconds(attackInterval);
        }
    }

    private void FireArrow(BaseObject target, Transform arrowStartPos)
    {
        PoolObject arrow = PoolManager.Instance.Spawn("Arrow");
        arrow.transform.position = arrowStartPos.position;
        arrow.transform.rotation = arrowStartPos.rotation;
        arrow.GetComponent<Projectile>().Init(target, arrowPower);
    }

    IEnumerator SpellEffect()
    {
        while (true)
        {
            ApplySpell();
            yield return new WaitForSeconds(freezeDuration);
        }
    }

    private void ApplySpell()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, spellRadius);
        foreach (var hitCollider in hitColliders)
        {
            Unit unit = hitCollider.gameObject.GetComponent<Unit>();
            if (unit != null && !affectedUnits.Contains(unit))
            {
                affectedUnits.Add(unit);
                SlowDownMovement(unit);
                spellVFX.Play();
            }
        }
    }

    private void SlowDownMovement(Unit unit)
    {
        unit.moveSpeed /= 2;
    }

    private void OnDestroy()
    {
        foreach (Unit unit in affectedUnits)
        {
            RestoreSpeed(unit);
        }
        affectedUnits.Clear();
    }

    private void RestoreSpeed(Unit unit)
    {
        if (unit != null)
        {
            unit.moveSpeed *= 2;
        }
    }
}
