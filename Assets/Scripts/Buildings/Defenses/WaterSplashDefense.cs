using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplashDefense : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 5f;
    [SerializeField]
    private Transform splashOrigin;
    [SerializeField]
    private float splashInterval = 3f;
    [SerializeField]
    private float splashRange = 5f;
    [SerializeField]
    private int splashDamage = 20;
    [SerializeField]
    private GameObject waterSplashPrefab;

    private Transform targetEnemy;

    private void Start()
    {
        StartCoroutine(DefendWithWater());
    }

    private IEnumerator DefendWithWater()
    {
        while (true)
        {
            FindClosestEnemy();
            yield return new WaitForSeconds(splashInterval);

            if (targetEnemy != null)
            {
                RotateTowardsEnemy();
                SplashWater(targetEnemy.position);
            }
        }
    }

    private void FindClosestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, splashRange);
        float closestDistance = Mathf.Infinity;

        foreach (var collider in hitColliders)
        {
            Unit enemyUnit = collider.GetComponent<Unit>();
            if (enemyUnit != null)
            {
                float distance = Vector3.Distance(transform.position, enemyUnit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetEnemy = enemyUnit.transform;
                }
            }
        }
    }

    private void RotateTowardsEnemy()
    {
        if (targetEnemy != null)
        {
            Vector3 targetDirection = targetEnemy.position - splashOrigin.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            splashOrigin.rotation = Quaternion.Slerp(splashOrigin.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void SplashWater(Vector3 targetPosition)
    {
        GameObject splash = Instantiate(waterSplashPrefab, splashOrigin.position, Quaternion.identity);
        splash.transform.LookAt(targetPosition);
        Destroy(splash, 2f);

        Unit[] enemyUnits = FindObjectsOfType<Unit>();
        foreach (var enemyUnit in enemyUnits)
        {
            if (Vector3.Distance(splashOrigin.position, enemyUnit.transform.position) <= splashRange)
            {
                enemyUnit.OnHit(splashDamage);
            }
        }
    }
}
