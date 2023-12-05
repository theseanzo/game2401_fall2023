using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; //this is our tweening library
public class SpawningProjectile : PoolObject
{
    [SerializeField]
    float speed = 5f;

    [SerializeField] private Unit _unit;
    [SerializeField] private float _spawnDistance = 3f;
    private Vector3 startPos;
    private int attackPower;
    private BaseObject attackTarget;
    public void Init(BaseObject target, int power)
    {
        attackPower = power;
        attackTarget = target;
        //find the location we are attacking
        Vector3 targetPos =  target.GetComponent<Collider>().ClosestPoint(startPos);
        targetPos = targetPos + (-_spawnDistance * (targetPos - startPos).normalized); // gets point between starting position and target that is X distance away from the building
        transform.LookAt(targetPos);
        Tweener moveTween = transform.DOMove(targetPos, speed); //notice that we don't provide a duration and we only provide a speed; the reason for this is that we don't want a fixed duration in case objects are farther or closer
        moveTween.SetSpeedBased(true);//make sure that we are setting the tween to be speed based
        moveTween.OnComplete(OnProjectileArrived);
    }
    private void OnProjectileArrived() //we want this function to be called when we actually make it to our target
    {
        Instantiate(_unit, transform.position, transform.rotation);
        OnDeSpawn();
    }
    // Start is called before the first frame update
    void Start()
    {
        startPos = new Vector3(transform.position.x,.01f,transform.position.z );
    }

    // Update is called once per frame
    void Update()
    {

    }
}
