using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SpellEffect : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f; // Speed at which the spell moves
    public float radius = 5f; // Radius of the spell's enrage effect
    public float rageDuration = 10f; // Duration of the enrage effect
    public LayerMask targetLayer; // Layer mask to specify which layers the spell can interact with (units)

    void Start()
    {
        StartRageEffect();
    }

    private void StartRageEffect()
    {
        Vector3 targetPosition = GetMouseClickPosition();
        MoveSpellToPosition(targetPosition);
        ScheduleSpellDestruction();
    }

    private Vector3 GetMouseClickPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private void MoveSpellToPosition(Vector3 targetPosition)
    {
        transform.DOMove(targetPosition, speed).SetSpeedBased(true).OnComplete(OnSpellArrived);
    }

    private void OnSpellArrived()
    {
        EnrageUnits();
        Debug.Log("Rage spell activated");
    }

    private void EnrageUnits()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, targetLayer);
        foreach (var hitCollider in hitColliders)
        {
            EnrageUnit(hitCollider);
        }
    }

    private void EnrageUnit(Collider hitCollider)
    {
        Unit unit = hitCollider.GetComponent<Unit>();
        if (unit != null)
        {
            // Assuming Unit has a method to become enraged
            unit.BecomeEnraged(rageDuration);
        }
    }

    private void ScheduleSpellDestruction()
    {
        Destroy(gameObject, rageDuration);
    }
}
