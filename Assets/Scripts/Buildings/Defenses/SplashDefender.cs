using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashDefender : Unit
{
    [SerializeField]
    Transform waterStartPos;

    [SerializeField]
    private float waterDespawnTimer = 3f;

    // Start is called before the first frame update
    public override void OnAttackActionEvent()
    {
        base.OnAttackActionEvent();
        Debug.Log("Splash Defender OnAttackActionEvent");

        if (attackTarget != null)
        {
            PoolObject waterSplash = PoolManager.Instance.Spawn("WaterSplash");

            if (waterSplash != null)
            {
                Debug.Log("Spawning water");
                waterSplash.transform.position = waterStartPos.position;
                waterSplash.transform.rotation = waterStartPos.rotation;
                waterSplash.GetComponent<Projectile>().Init(attackTarget, attackPower);

                // Start the despawn timer for the magic spell
                StartCoroutine(DespawnWaterSplash(waterSplash));
            }
        }
    }


    IEnumerator DespawnWaterSplash(PoolObject waterSplash)
    {
        yield return new WaitForSeconds(waterDespawnTimer);
        waterSplash.OnDeSpawn();
    }
}