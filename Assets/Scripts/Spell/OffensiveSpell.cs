using System.Collections;
using UnityEngine;

public class OffensiveSpell : MonoBehaviour
{
    public float spellRadius = 5f;
    public float spellDuration = 5f;
    public GameObject spellVFXPrefab;

    private void Update()
    {
        // Check for input to cast the AOE spell
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CastSpell();
        }
    }

    private void CastSpell()
    {
        StartCoroutine(ApplySpell());
    }

    private IEnumerator ApplySpell()
    {
        // Create a sphere at the cursor position with the specified radius
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(mouseRay, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            Vector3 spellPosition = hitInfo.point;

            GameObject spellVFX = Instantiate(spellVFXPrefab, spellPosition, Quaternion.identity);
            yield return new WaitForSeconds(0.1f); // Give a short delay for visualization

            // Detect units within the AOE radius and stop their movement
            Collider[] colliders = Physics.OverlapSphere(spellPosition, spellRadius);
            foreach (Collider collider in colliders)
            {
                Unit unit = collider.GetComponent<Unit>();
                if (unit != null)
                {
                    unit.StopMovementForDuration(spellDuration);
                }
            }

            // Wait for the specified duration before allowing units to move again
            yield return new WaitForSeconds(spellDuration);

            // Resume the movement of affected units
            foreach (Collider collider in colliders)
            {
                Unit unit = collider.GetComponent<Unit>();
                if (unit != null)
                {
                    unit.ResumeMovement();
                }
            }
            Destroy(spellVFX);
        }
    }
}