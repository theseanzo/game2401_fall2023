using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SpellEffect : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    public float radius = 5f;
    public float effectDuration = 10f;
    public LayerMask targetLayer;

    void Start()
    {
        StartSpellEffect();
    }

    private void StartSpellEffect()
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
        ApplyEffect();
        Debug.Log("Spell arrived at target");
    }

    private void ApplyEffect()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, targetLayer);
        foreach (var hitCollider in hitColliders)
        {
            ProcessCollider(hitCollider);
        }
    }

    private void ProcessCollider(Collider hitCollider)
    {
        Building building = hitCollider.GetComponent<Building>();
        if (building != null)
        {
            building.IsOverlapping = true;
            StartCoroutine(ResetColor(building));
        }
    }

    IEnumerator ResetColor(Building building)
    {
        yield return new WaitForSeconds(effectDuration);
        building.IsOverlapping = false;
    }

    private void ScheduleSpellDestruction()
    {
        Destroy(gameObject, effectDuration);
    }
}
