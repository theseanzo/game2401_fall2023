using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class OffensiveSpell : MonoBehaviour
{
    public GameObject spellIndicatorPrefab;
    public ParticleSystem spellParticlePrefab;
    public float spellRadius = 5f;

    private GameObject spellIndicator;
    private bool isKeyPressed = false;

    public UnityEvent onSpellCast;
    public BaseObject baseObject;

    // Define the color to change buildings to when hit
    public Color hitColor = Color.red;
    // Define the duration to keep the color changed
    public float hitDuration = 1.0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isKeyPressed = true;
            onSpellCast.Invoke();
        }
        else if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            isKeyPressed = false;
        }

        if (isKeyPressed)
        {
            ShowSpellIndicator();
        }
        else
        {
            HideSpellIndicator();
        }
    }

    void ShowSpellIndicator()
    {
        // Ensure the indicator prefab is assigned
        if (spellIndicatorPrefab == null)
        {
            Debug.LogError("Spell indicator prefab is not assigned!");
            return;
        }

        // Check if the indicator is not already instantiated
        if (spellIndicator == null)
        {
            // Instantiate the spell indicator
            spellIndicator = Instantiate(spellIndicatorPrefab);
        }

        // Position the spell indicator at the mouse position
        Vector3 mousePosition = GetMouseWorldPosition();
        spellIndicator.transform.position = new Vector3(mousePosition.x, 1f, mousePosition.z);

        // Scale the indicator based on the spell radius
        float scale = spellRadius / 5f; // Adjust the scale factor based on your game requirements
        spellIndicator.transform.localScale = new Vector3(scale, scale, 1f);

        // Set the indicator to be visible
        spellIndicator.SetActive(true);
        Debug.Log("Spell indicator instantiated at: " + spellIndicator.transform.position);
        Debug.Log("Spell indicator scale: " + spellIndicator.transform.localScale);
    }

    void HideSpellIndicator()
    {
        // Check if the indicator is instantiated and visible
        if (spellIndicator != null && spellIndicator.activeSelf)
        {
            // Set the indicator to be invisible
            spellIndicator.SetActive(false);
        }
    }

    void CheckBuildingCollisions()
    {
        Collider[] colliders = Physics.OverlapBox(spellIndicator.transform.position, spellIndicator.transform.localScale / 2f, spellIndicator.transform.rotation, LayerMask.GetMask("Building"));

        foreach (Collider collider in colliders)
        {
            Building building = collider.GetComponent<Building>();

            if (building != null)
            {
                // Change the building color temporarily
                StartCoroutine(ChangeBuildingColor(collider, hitColor, hitDuration));

                // Reduce the health of the building by 20%
                int reducedHealth = Mathf.CeilToInt(building.Health * 0.8f);

                if (reducedHealth <= 0)
                {
                    building.OnDie();
                }
                else
                {
                    building.SetHealth(reducedHealth); // Update the building health
                }
            }
        }
    }


    IEnumerator ChangeBuildingColor(Collider collider, Color color, float duration)
    {
        // Save the original color
        Color originalColor = collider.GetComponent<Renderer>().material.color;

        // Change the building color
        collider.GetComponent<Renderer>().material.color = color;

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Restore the original color
        collider.GetComponent<Renderer>().material.color = originalColor;
    }

    Vector3 GetMouseWorldPosition()
    {
        // Get the mouse position in screen space
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to world space
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));

        return mousePosition;
    }
}
