using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ArrowTower : Building
{

    [SerializeField]
    Transform arrowStartPos;

    [SerializeField]
    private int arrowPower;

    [SerializeField]
    private float attackInterval;

    private List<BaseObject> attackTarget = new List<BaseObject>();

    [SerializeField]
    private float colliderRadius;

    private ParticleSystem ps;

    protected override void Start()
    {
        base.Start();
        ps = GetComponentInChildren<ParticleSystem>();

        //Adjusting Radius depending on inspector
        var sh = ps.shape;
        sh.radius = colliderRadius;

        //Start the lookout for enemies
        StartCoroutine(AttackEnemies(attackInterval));
    }

    private void Update()
    {
        //Identify if enemy has entered the set radius of action
        Collider [] hitColliders = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z), colliderRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.GetComponent<Unit>())
            {
                if (!attackTarget.Contains(hitCollider.gameObject.GetComponent<BaseObject>()))
                {
                    attackTarget.Add(hitCollider.gameObject.GetComponent<BaseObject>());
                    FireArrow(hitCollider.gameObject.GetComponent<BaseObject>());
                }

            }
        }
    }

    IEnumerator AttackEnemies(float waitTime)
    {
        // Fire arrow every x seconds after enemy is inside the radius
        while (true)
        {
            foreach (BaseObject target in attackTarget)
            {
                if (target != null)
                {
                    FireArrow(target);
                }
            }

            yield return new WaitForSeconds(waitTime);
        }

    }

    private void FireArrow(BaseObject target)
    {
        // Fire arrow
        PoolObject arrow = PoolManager.Instance.Spawn("Arrow"); //grab an arrow
        arrow.transform.position = arrowStartPos.position;
        arrow.transform.rotation = arrowStartPos.rotation; //set up our arrow to follow the arrowStartPos and rotation
        arrow.GetComponent<Projectile>().Init(target, arrowPower);

    }

}
