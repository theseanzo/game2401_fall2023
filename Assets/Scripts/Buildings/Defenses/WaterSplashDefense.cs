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
    private float splashSpeed = 10f; // Adjust the speed as needed
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
            SplashWater();
        }
    }

    private void SplashWater()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, splashRange);

        foreach (var collider in hitColliders)
        {
            Unit enemyUnit = collider.GetComponent<Unit>();
            if (enemyUnit != null)
            {
                if (Vector3.Distance(splashOrigin.position, enemyUnit.transform.position) <= splashRange)
                {
                    GameObject splash = Instantiate(waterSplashPrefab, splashOrigin.position, Quaternion.identity);

                    // Calculate the direction towards the enemy
                    Vector3 splashDirection = (enemyUnit.transform.position - splash.transform.position).normalized;

                    // Set the velocity of the splash object to shoot towards the enemy
                    splash.GetComponent<Rigidbody>().velocity = splashDirection * splashSpeed;

                    Destroy(splash, 2f);

                    enemyUnit.OnHit(splashDamage);
                }
            }
        }
    }
}

