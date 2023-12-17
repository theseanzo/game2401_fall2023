using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    public float health = 40; // Player's health

    // Update is called once per frame
    void Update()
    {
        // Get input for movement
        float horizontalInput = Input.GetAxis("Horizontal"); // A/D keys or left/right arrows
        float verticalInput = Input.GetAxis("Vertical"); // W/S keys or up/down arrows

        // Calculate the movement direction
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Apply movement in the direction
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Destroy the player if health reaches zero
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
