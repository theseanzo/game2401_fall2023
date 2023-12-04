using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Unit
{

    [SerializeField]
    Transform fireBallPos;

    [SerializeField]
    float rageTime = 5f;
    [SerializeField]
    float coolDown = 10f;
    [SerializeField]
    int ragedAttckPower = 10;
    int defaultAttackPower;
    float timer = 0f;
    // Start is called before the first frame update
    private void Awake()
    {
        defaultAttackPower = attackPower;
    }
    
    public override void OnAttackActionEvent()
    {

        base.OnAttackActionEvent();
        if (attackTarget != null)
        {
            PoolObject fireball = PoolManager.Instance.Spawn("FireBall");
            fireball.transform.position = fireBallPos.position;
            fireball.transform.rotation = fireBallPos.rotation;
            fireball.GetComponent<Projectile>().Init(attackTarget, attackPower);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        Rage();
    }

    private void Rage()
    {
        if (timer >= coolDown)
        {
            attackPower = ragedAttckPower;
            if (timer >= rageTime + coolDown)
            {
                attackPower = defaultAttackPower;
                timer = 0f;
            }
        }
    }

}
