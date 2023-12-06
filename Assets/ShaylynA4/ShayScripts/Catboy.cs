using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catboy : Unit
{
    // He points with such sass and fervor that he does damage to buildings.

    public GameObject Cat;

    [SerializeField]
    private float _firstNumber = -10f;

    [SerializeField]
    private float _secondNumber = 11f;

    [SerializeField]
    private float _thirdNumber = 5f;

    public override void OnAttackActionEvent() // Overriding the parent's function
    {
        base.OnAttackActionEvent(); // Completes the parent's OnAttackActionEvent
        if (attackTarget != null) // Checks to make sure there is a target
        {
            attackTarget.OnHit(attackPower); // Calculates the damage (I think)

            Vector3 spawnPosition = new Vector3(Random.Range(_firstNumber, _secondNumber), _thirdNumber, Random.Range(_firstNumber, _secondNumber)); // Calculates where to spawn the cat

            Instantiate(Cat, spawnPosition, Quaternion.identity); // Instantiates the cat
        }
    }

}
