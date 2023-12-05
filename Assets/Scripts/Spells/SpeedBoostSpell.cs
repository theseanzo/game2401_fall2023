using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostSpell : MonoBehaviour
{
    [Header("Speed Boost Settings")]
    [SerializeField]
    private float speedBoostMultiplier = 1.5f;
    [SerializeField]
    private float spellDuration = 5f;
    [SerializeField]
    private float spellRadius = 5f;
    [SerializeField]
    private GameObject speedParticlesPrefab;

    [Header("Circle Drawing Settings")]
    [SerializeField]
    private Material circleMaterial; // Assign a material for the circle in the Inspector.

    private GameObject newSpeedParticlesPrefab;

    private void Update()
    {
        // Check for input to cast the spell
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CastSpeedBoostSpell();
        }
    }

    private void CastSpeedBoostSpell()
    {
        // Raycast to the mouse position on the terrain (adjust the layer mask as needed).
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            // Instantiate particle system at the spell's position.
            InstantiateSpellEffect(hit.point);

            // Draw a circle at the spell's position.
            DrawCircle(hit.point);

            // Apply the speed boost to units in the area.
            StartCoroutine(ApplySpeedBoost(hit.point));
        }
    }

    private void InstantiateSpellEffect(Vector3 position)
    {
        // Instantiate your particle system or visual effect at the specified position.
        newSpeedParticlesPrefab = Instantiate(speedParticlesPrefab, position, Quaternion.identity);
    }

    private void DrawCircle(Vector3 center)
    {
        int segments = 64;

        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (!lineRenderer)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = circleMaterial;

        float deltaTheta = (2f * Mathf.PI) / segments;
        float theta = 0f;

        for (int i = 0; i < segments + 1; i++)
        {
            float x = center.x + spellRadius * Mathf.Cos(theta);
            float z = center.z + spellRadius * Mathf.Sin(theta);

            Vector3 pos = new Vector3(x, center.y, z);
            lineRenderer.SetPosition(i, pos);

            theta += deltaTheta;
        }
    }

    private IEnumerator ApplySpeedBoost(Vector3 spellPosition)
    {
        // Cast a sphere and get all hits within the radius
        RaycastHit[] hits = Physics.SphereCastAll(spellPosition, spellRadius, Vector3.up, Mathf.Infinity);

        Debug.Log("Number of hits found: " + hits.Length);

        foreach (RaycastHit hit in hits)
        {
            Collider collider = hit.collider;
            Unit unit = collider.GetComponent<Unit>();

            if (unit != null)
            {
                Debug.Log("Found Unit: " + unit.gameObject.name);

                unit.StartCoroutine(SpeedBoostEffect(unit));
            }
        }

        // Wait for the spell duration.
        yield return new WaitForSeconds(spellDuration);

        // Clear the circle drawing after the duration.
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer)
        {
            Destroy(lineRenderer);
        }

        // Remove the speed boost after the duration.
        foreach (RaycastHit hit in hits)
        {
            Unit unit = hit.collider.GetComponent<Unit>();
            if (unit != null)
            {
                unit.moveSpeed /= speedBoostMultiplier;
            }
        }

        // Destroy the instantiated particle system.
        Destroy(newSpeedParticlesPrefab);
    }

    private IEnumerator SpeedBoostEffect(Unit unit)
    {
        unit.moveSpeed *= speedBoostMultiplier;

        yield return new WaitForSeconds(spellDuration);

        unit.moveSpeed /= speedBoostMultiplier;
    }
}
