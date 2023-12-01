using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SpellEffect : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f; // speed of the spell movement
    public float radius = 5f; // radius of the spell's effect
    public float effectDuration = 10f; // duration of the spell's effect
    public LayerMask targetLayer; // layer mask to specify which layers the spell can interact with

    void Start()
    {
        // get the position of the mouse click and move the spell to that position
        Vector3 targetPosition = GetMouseClickPosition();
        MoveSpellToPosition(targetPosition);
        // destroy the spell object after its duration ends
        Destroy(gameObject, effectDuration);
    }

    private Vector3 GetMouseClickPosition()
    {
        // create a ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // perform a raycast to check if it hits anything
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point; // return the hit position
        }
        return Vector3.zero; // return zero vector if nothing is hit
    }

    private void MoveSpellToPosition(Vector3 targetPosition)
    {
        // animate the movement of the spell to the target position
        Tweener moveTween = transform.DOMove(targetPosition, speed).SetSpeedBased(true);
        moveTween.OnComplete(OnSpellArrived); // call OnSpellArrived when movement is complete
    }

    private void OnSpellArrived()
    {
        // apply the spell's effect when it arrives at the target position
        ApplyEffect();
        Debug.Log("Spell arrived at target");
    }

    private void ApplyEffect()
    {
        // find all colliders within the spell's radius that are on the target layer
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, targetLayer);
        foreach (var hitCollider in hitColliders)
        {
            // get the Building component of the collider and apply the effect
            Building building = hitCollider.GetComponent<Building>();
            if (building != null)
            {
                building.IsOverlapping = true;
                StartCoroutine(ResetColor(building, effectDuration)); // reset the color after a delay
            }
        }
    }

    IEnumerator ResetColor(Building building, float delay)
    {
        // wait for the specified delay and then reset the building's overlapping status
        yield return new WaitForSeconds(delay);
        building.IsOverlapping = false;
    }
}
