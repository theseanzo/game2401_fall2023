using System.Collections;
using UnityEngine;

public class ParalyzingFog : MonoBehaviour
{
    [SerializeField]
    float reducedSpeed = 0f;

    private Unit affectedUnit; // Variable to keep the unit will be affected by the spell

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the tag "Unit"
        if (other.CompareTag("Unit"))
        {
            Debug.Log("Collided");
            // Get the movement component of the unit (adjust as needed)
            affectedUnit = other.GetComponent<Unit>();

            // Check if the object has the movement component
            if (affectedUnit != null)
            {
                // Reduce speed to 0
                affectedUnit.moveSpeed = reducedSpeed;
                float delay = 5.0f;
                StartCoroutine(RestoreSpeedAfterDelay(delay));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object that exited the trigger has the tag "Unit"
        if (other.CompareTag("Unit"))
        {
            // Reset the affectedUnit variable to null when the unit exits the trigger
            affectedUnit = null;
        }
    }

    private void OnDestroy()
    {
        // Check if there is a unit affected by the fog before destroying it
        if (affectedUnit != null)
        {
            affectedUnit.RestoreOriginalSpeed();
        }
    }

    private IEnumerator RestoreSpeedAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Check if the unit still exists before restoring speed
        if (affectedUnit != null)
        {
            affectedUnit.RestoreOriginalSpeed();
        }
    }
}
