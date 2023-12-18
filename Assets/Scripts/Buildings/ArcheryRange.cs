using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcheryRange : Building
{
    [SerializeField] private Transform _arrowSpawn;
    [SerializeField] private int _attackPower;
    [SerializeField] private float _attackInterval;
    private float lastAttackTime = 0f;
    [SerializeField] private float _attackRange;
    private Coroutine currentState;
    private Unit _target;

    private void SetState(IEnumerator newState) //when we change states, we stop our previous coroutine and then initialize a new one. Technically this can be done with "StopAllCoroutines()" because we only have one coroutine running, but in the case that we had more, we will only a stop a specific coroutine.
    {
        if (currentState != null)
        {
            StopCoroutine(currentState);
        }
        currentState = StartCoroutine(newState);
    }

    protected override void Start()
    {
        base.Start();
        SetState(OnIdle());
    }

    private void FixedUpdate()
    {
        //SearchForTarget();
        if(GameManager.Instance.CurrentState == GameState.Attacking)
        {
            SearchForTarget();
        }
    }

    IEnumerator OnIdle()
    {
        yield return null;
    }

    IEnumerator OnAttack(Unit target)
    {
        
        while (true)
        {
            //we want to attack our target after our interval 

            lastAttackTime += Time.deltaTime;
            //what do do if our building has been destroyed? We idle again
            if (target == null)
            {
                SetState(OnIdle());
            }
            if (lastAttackTime >= _attackInterval)
            {
                lastAttackTime = 0;
                Attack(target);
                //Attack
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private void SearchForTarget()
    {
        Unit[] allUnits = FindObjectsOfType<Unit>(); 
        float shortestDistance = Mathf.Infinity;
        Unit closestUnit = null; //currently we don't have a closest building so it is null
        foreach (Unit unit in allUnits)
        {
            float distance = Vector3.Distance(transform.position, unit.transform.position); //get the distance from our unit to the building
            if (distance < shortestDistance && distance < _attackRange)
            {
                shortestDistance = distance;
                closestUnit = unit;
            }
        }
        if (closestUnit != null) 
        {
            SetState(OnAttack(closestUnit));
        }
    }

    private void Attack(Unit target)
    {
        PoolObject arrow = PoolManager.Instance.Spawn("Arrow"); //grab an arrow
        arrow.transform.position = _arrowSpawn.position;
        arrow.transform.rotation = _arrowSpawn.rotation; //set up our arrow to follow the arrowStartPos and rotation
        arrow.GetComponent<Projectile>().Init(target, _attackPower);
    }
}
