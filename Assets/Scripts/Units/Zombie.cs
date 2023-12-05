using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Unit
{
    [SerializeField] private float _slowMoveSpeedMultiplier = 0.5f; // Multiplier to reduce the move speed
    [SerializeField] private int _increasedAttackPower = 10;
    [SerializeField] private int _interval = 5; // default interval set to 5 seconds
    private float _timer = 0f;

    protected override void Start()
    {
        base.Start();
        moveSpeed *= _slowMoveSpeedMultiplier; // Reducing the move speed
    }

    // Update is called once per frame
    void Update()
    {
        HandleAttackPowerIncrease();
    }

    /// <summary>
    /// Increases the attack power of the zombie at regular intervals.
    /// </summary>
    private void HandleAttackPowerIncrease()
    {
        _timer += Time.deltaTime;
        if (_timer >= _interval)
        {
            attackPower += _increasedAttackPower;
            _timer = 0f;
        }
    }
}
