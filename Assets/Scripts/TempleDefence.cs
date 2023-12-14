using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class TempleDefence : MonoBehaviour
{
    [SerializeField] private int attackPower = 50;
    [SerializeField] private float attackInterval = 2f;  
    [SerializeField] private GameObject spearBarrel;
    [SerializeField] GameObject targetParticle;

    private Unit AttackTarget;
    private float timer;

    void Start()
    {
        timer = attackInterval;
    }

    private void OnTriggerStay(Collider other)
    {
        AttackTarget = other.GetComponent<Unit>();

        if (AttackTarget != null)
        {
            timer += Time.deltaTime;
            

            
            if (timer >= attackInterval)
            {
                GameObject UnitEffect = Instantiate(targetParticle, AttackTarget.transform.position, Quaternion.identity);
                UnitEffect.transform.parent = AttackTarget.transform;
                timer = 0;
                PoolObject arrow = PoolManager.Instance.Spawn("Spear");
                arrow.transform.position = spearBarrel.transform.position;
                arrow.transform.rotation = spearBarrel.transform.rotation;
                arrow.GetComponent<Projectile>().Init(AttackTarget, attackPower);
            }
        }
    }

    void Update()
    {

    }
}