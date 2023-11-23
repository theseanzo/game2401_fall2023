using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageSpell : MonoBehaviour
{
    private ParticleSystem particleSystem;

    [SerializeField]
    private float increaseMoveSpeed;
    [SerializeField]
    private int increaseAttackPower;
    [SerializeField]
    private float decreaseeAttackInterval;

    private Unit unit;
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (particleSystem == null)
        {
            Destroy(gameObject);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        if (other.gameObject.GetComponent<Archer>() != null)
        {
            unit = other.gameObject.GetComponent<Archer>();
            unit.moveSpeed += increaseMoveSpeed;
            unit.attackPower += increaseAttackPower;
            unit.attackInterval -= decreaseeAttackInterval;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Unit>() != null)
        {
            unit = other.gameObject.GetComponent<Unit>();
            unit.moveSpeed -= increaseMoveSpeed;
            unit.attackPower -= increaseAttackPower;
            unit.attackInterval += decreaseeAttackInterval;
        }
    }
}
