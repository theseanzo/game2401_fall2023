using System.Collections.Generic;
using UnityEngine;

public class FreezeSpell : MonoBehaviour
{
    [SerializeField]
    private GameObject slowEffectPrefab;

    [SerializeField]
    private float slowingAmount;

    [SerializeField]
    private float duration;

    [SerializeField]
    private float effectRadius;

    private Dictionary<Unit, GameObject> affectedUnits = new();

    private void Update()
    {
        ApplyEffect();
        UpdateDuration();
    }

    /// <summary>
    /// Applies the slow effect to units within the spell's radius.
    /// </summary>
    private void ApplyEffect()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, effectRadius);
        foreach (Collider collider in colliders)
        {
            Unit unit = collider.gameObject.GetComponent<Unit>();
            if (unit != null && !affectedUnits.ContainsKey(unit))
            {
                ModifyUnitStats(unit, slowingAmount);
                GameObject effectInstance = Instantiate(slowEffectPrefab, unit.transform.position, Quaternion.identity, unit.transform);
                affectedUnits.Add(unit, effectInstance);
            }
        }
    }

    /// <summary>
    /// Updates the duration of the spell and cleans up after it expires.
    /// </summary>
    private void UpdateDuration()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            foreach (var pair in affectedUnits)
            {
                ModifyUnitStats(pair.Key, -slowingAmount);
                Destroy(pair.Value);
            }
            affectedUnits.Clear();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Modifies the unit's stats for the slow effect.
    /// </summary>
    /// <param name="unit">The unit to modify.</param>
    /// <param name="amount">The amount to modify the unit's stats.</param>
    private void ModifyUnitStats(Unit unit, float amount)
    {
        // Ensure that the unit's speed is only reduced and not set to a negative value
        unit.moveSpeed = Mathf.Max(unit.moveSpeed - amount, 0);

        // Similarly, ensure the attack interval is only increased and stays above a minimum threshold
        unit.attackInterval = Mathf.Max(unit.attackInterval + amount, 1);
    }
}
