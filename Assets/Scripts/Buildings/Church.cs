using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Church : Building
{
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private float fireRate = 2.5f;
    [SerializeField] private PlayerController playerController;

    // Enum to track the current state
    private enum State
    {
        LookingForEnemies,
        Attacking
    }

    private State currentState = State.LookingForEnemies;
    public UnityEvent HolyExplosion;

    private bool isAttacking = false; // Track if currently attacking

    // Set initial health
    private void Start()
    {
        Health = 200;

        // Ensure the SphereCollider is set up as a trigger in the Inspector
        SphereCollider collider = GetComponent<SphereCollider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
        else
        {
            Debug.LogError("SphereCollider component not found!");
        }
    }

    // When an enemy enters the collider, start attacking
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy entered the collider! Attacking now.");
            currentState = State.Attacking;

            // Start the attack routine if not already attacking
            if (!isAttacking)
            {
                isAttacking = true;
                StartCoroutine(AttackRoutine());
            }
        }
    }

    // When an enemy exits the collider, stop attacking
    private void OnTriggerExit(Collider other)
    {
        currentState = State.LookingForEnemies;
        Debug.Log("Something exited the collider. Looking for enemies again.");
        isAttacking = false; // Reset attacking state when the enemy leaves
    }

    // Attack routine that damages the player and invokes a Holy Explosion
    private IEnumerator AttackRoutine()
    {
        while (currentState == State.Attacking && isAttacking)
        {
            Debug.Log("Holy explosion coming!");
            playerController.health -= damage; // Reduce player health
            HolyExplosion.Invoke(); // Invoke Holy Explosion event
            yield return new WaitForSeconds(fireRate); // Wait for next attack
        }
    }
}
