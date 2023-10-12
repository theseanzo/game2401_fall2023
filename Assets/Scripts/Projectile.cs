using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; //this is our tweening library
public class Projectile : PoolObject
{
    [SerializeField]
    float speed = 5f;

    private int attackPower;
    private BaseObject attackTarget;
    public void Init(BaseObject target, int power)
    {
        attackPower = power;
        attackTarget = target;
        //find the location we are attacking
        Vector3 targetPos = target.GetComponent<Collider>().bounds.center;
        transform.LookAt(targetPos);
        Tweener moveTween = transform.DOMove(targetPos, speed); //notice that we don't provide a duration and we only provide a speed; the reason for this is that we don't want a fixed duration in case objects are farther or closer
        moveTween.SetSpeedBased(true);//make sure that we are setting the tween to be speed based
        moveTween.OnComplete(OnProjectileArrived);
    }
    private void OnProjectileArrived() //we want this function to be called when we actually make it to our target
    {
        attackTarget?.OnHit(attackPower);//the ? checks to make sure our attackTarget is not null
        OnDeSpawn();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
