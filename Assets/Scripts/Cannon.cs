using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header("Cannon Settings")]
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private int attackPower = 1;

    [Header("Effects")]
    [SerializeField] private ParticleSystem shootEffect;
    [SerializeField] private AudioClip shootSound;

    private Unit targetUnit;
    private float fireCountdown = 0f;
    private AudioSource audioSource;

    void Update()
    {
        RotateTowardsTarget();

        if (fireCountdown <= 0f)
        {
            if (targetUnit != null)
            {
                PoolObject cannon = PoolManager.Instance.Spawn("Cannon");
                cannon.transform.position = firePoint.position;
                cannon.transform.rotation = firePoint.rotation;
                cannon.GetComponent<Projectile>().Init(targetUnit, attackPower);

                InstantiateShootEffect();
                PlayShootSound();
            }
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;

        DetectUnits();
    }

    void RotateTowardsTarget()
    {
        if (targetUnit != null)
        {
            Vector3 direction = targetUnit.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void DetectUnits()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<Unit>())
            {
                targetUnit = collider.GetComponent<Unit>();
                return;
            }
        }

        targetUnit = null;
    }

    void InstantiateShootEffect()
    {
        if (shootEffect != null)
        {
            ParticleSystem newEffect = Instantiate(shootEffect, firePoint.position, firePoint.rotation);
            Destroy(newEffect.gameObject, newEffect.main.duration);
        }
    }

    void PlayShootSound()
    {
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}
