using UnityEngine;
using UnityEngine.EventSystems;

public class CamControls : MonoBehaviour
{
    public float panSpeed = 8f; // the camera panning speed
    public float arrowKeySpeed = 9f; // speed with arrow keys.
    public Collider2D boundingCollider; // The collider that defines the camera's bounding area.
    public float minZoom = 3f; // Minimum zoom level
    public float maxZoom = 200f; // Maximum zoom level
    public float zoomSpeed = 5f; // Zoom speed
    public float boundingBoxPadding = 1f;
    public float rotationSpeed = 27f; // Speed of camera rotation
    public float scrollSpeed = 20f;

    private float minX, maxX, minY, maxY;
    private float rotationAngle = 0f; // Current camera rotation angle around Y-axis
    private Camera camera;

    void Start()
    {
        camera = Camera.main;

        if (boundingCollider != null)
        {
            Bounds bounds = boundingCollider.bounds;
            camera = Camera.main;

            // Calculate the camera's position boundaries based on the bounding area.
            minX = bounds.min.x + base.GetComponent<Camera>().orthographicSize * base.GetComponent<Camera>().aspect;
            maxX = bounds.max.x - base.GetComponent<Camera>().orthographicSize * base.GetComponent<Camera>().aspect;
            minY = bounds.min.y + base.GetComponent<Camera>().orthographicSize;
            maxY = bounds.max.y - base.GetComponent<Camera>().orthographicSize;

            Vector3 initialPosition = transform.position;
            initialPosition.x = Mathf.Clamp(initialPosition.x, minX, maxX);
            initialPosition.y = Mathf.Clamp(initialPosition.y, minY, maxY);
            transform.position = initialPosition;
        }
    }

    void Update()
    {
        // Check if the cursor is over a UI element.
        bool isCursorOverUI = EventSystem.current.IsPointerOverGameObject();

        if (!isCursorOverUI)
        {
            mouseCameraPan();
            cameraMovementArrow();
            zoomCameraScroll();
            rotateCameraWithKeys();
        }

        cameraClampPosition();
        clampCameraZoom();
    }

    void mouseCameraPan()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 moveDirection = Vector3.zero;

        if (mousePosition.x <= 0)
            moveDirection.x = -1;
        else if (mousePosition.x >= Screen.width - 1)
            moveDirection.x = 1;

        if (mousePosition.y <= 0)
            moveDirection.y = -1;
        else if (mousePosition.y >= Screen.height - 1)
            moveDirection.y = 1;

        moveDirection.z = 0; // Set the z-component to 0 to prevent panning along the z-axis.


        transform.Translate(moveDirection * panSpeed * Time.deltaTime);
    }

    void cameraMovementArrow()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(moveDirection * arrowKeySpeed * Time.deltaTime);
    }

    void cameraClampPosition()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        transform.position = clampedPosition;
    }

    void zoomCameraScroll()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float newZoom = camera.fieldOfView - scrollInput * scrollSpeed * Time.deltaTime;
        camera.fieldOfView = Mathf.Clamp(newZoom, minZoom, maxZoom);
    }

    void clampCameraZoom()
    {
        Camera camera = Camera.main;
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, minZoom, maxZoom);
    }

    void rotateCameraWithKeys()
    {
        // Rotate the camera clockwise with 'E' key and counter-clockwise with 'Q' key.
        if (Input.GetKey(KeyCode.E))
            rotationAngle -= rotationSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.Q))
            rotationAngle += rotationSpeed * Time.deltaTime;

        
        transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
    }

}
