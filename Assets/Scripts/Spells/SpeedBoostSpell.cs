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
    private Material circleMaterial;

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
        // Raycast to the mouse position on the terrain
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            InstantiateSpellEffect(hit.point);

            DrawCircle(hit.point);

            StartCoroutine(ApplySpeedBoost(hit.point));
        }
    }

    private void InstantiateSpellEffect(Vector3 position)
    {
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

        Dictionary<Unit, float> originalSpeeds = new Dictionary<Unit, float>();

        foreach (RaycastHit hit in hits)
        {
            Collider collider = hit.collider;
            Unit unit = collider.GetComponent<Unit>();

            if (unit != null)
            {
                Debug.Log("Found Unit: " + unit.gameObject.name);

                // Store the original speed before applying the boost
                originalSpeeds[unit] = unit.moveSpeed;

                unit.StartCoroutine(SpeedBoostEffect(unit));
            }
        }

        yield return new WaitForSeconds(spellDuration);

        // Delete the circle drawing after the duration.
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer)
        {
            Destroy(lineRenderer);
        }

        // Go back to the original speed after the duration.
        foreach (var speed in originalSpeeds)
        {
            speed.Key.moveSpeed = speed.Value;
        }

        Destroy(newSpeedParticlesPrefab);
    }

    private IEnumerator SpeedBoostEffect(Unit unit)
    {
        unit.moveSpeed *= speedBoostMultiplier;

        yield return new WaitForSeconds(spellDuration);

        unit.moveSpeed /= speedBoostMultiplier;
    }
}
