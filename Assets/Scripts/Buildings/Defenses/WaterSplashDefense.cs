using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplashDefense : MonoBehaviour
{
    [SerializeField]
    private Transform splashOrigin; 
    [SerializeField]
    private float splashInterval = 3f; 
    [SerializeField]
    private float splashRange = 5f;
    [SerializeField]
    private int splashDamage = 20;
    [SerializeField]
    private float splashSpeed = 10f;
    [SerializeField]
    private GameObject waterSplashPrefab;

    private void Start()
    {
        StartCoroutine(DefendWithWater());
    }

    private IEnumerator DefendWithWater()
    {
        while (true)
        {
            yield return new WaitForSeconds(splashInterval);
            SplashWater(); // trigger the water splash
        }
    }

    private void SplashWater()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, splashRange); // find colliders within the splash range

        foreach (var collider in hitColliders)
        {
            Unit enemyUnit = collider.GetComponent<Unit>(); // get a unit component from the collider
            if (enemyUnit != null)
            {
                // Check if the enemy is within range
                if (Vector3.Distance(splashOrigin.position, enemyUnit.transform.position) <= splashRange)
                {
                    // instantiate a water splash 
                    GameObject splash = Instantiate(waterSplashPrefab, splashOrigin.position, Quaternion.identity);

                    // direction towards the enemy
                    Vector3 splashDirection = (enemyUnit.transform.position - splash.transform.position).normalized;

                    // set the velocity to shoot towards the enemy
                    splash.GetComponent<Rigidbody>().velocity = splashDirection * splashSpeed;

                    // instantiate another water splash effect at the position of the enemy
                    if (waterSplashPrefab != null)
                    {
                        Instantiate(waterSplashPrefab, enemyUnit.transform.position, Quaternion.identity);
                    }

                    Destroy(splash, 2f);

                    enemyUnit.OnHit(splashDamage);
                }
            }
        }
    }
}
