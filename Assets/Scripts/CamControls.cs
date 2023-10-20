using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamControls : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float panSpeed;          // Speed for panning the camera
    [SerializeField] private float speed;             // Speed for camera movement
    [SerializeField] private float rotationSpeed;     // Speed for camera rotation
    [SerializeField] private float zoomSpeed;         // Speed for zooming
    [SerializeField] private float smoothing;         // Smoothing for camera movement
    [SerializeField] private float rotationSmoothing; // Smoothing for camera rotation
    [SerializeField] private float panBorderThickness; // Thickness for edge panning

    [Header("Zoom Settings")]
    [SerializeField] private float maxZoomAngle;      // Maximum camera angle for zoomed in view
    [SerializeField] private float minZoomAngle;      // Minimum camera angle for zoomed out view
    [SerializeField] private Vector2 range = new Vector2(100, 100); // Movement range
    [SerializeField] private Vector2 zoomRange = new Vector2(30f, 70f); // Zoom range

    [Header("Camera Components")]
    [SerializeField] private Transform cameraHolder; // Transform of the camera holder

    private float targetAngle;              // Target camera angle
    private float currentAngle;             // Current camera angle
    private Vector3 targetPosition;         // Target camera position
    private Vector3 zoomTargetPosition;     // Target zoom position
    private Vector3 input;                  // Input for camera movement
    private Vector3 mouseWorldPosition;     // Mouse position in world space
    private float zoomInput;                // Input for zooming
    private Vector3 cameraDirection => transform.InverseTransformDirection(cameraHolder.forward);

    private EventSystem eventSystem; // Reference to the EventSystem

    // Start is called before the first frame update
    void Awake()
    {
        targetPosition = transform.position;
        targetAngle = transform.eulerAngles.y;
        currentAngle = targetAngle;
        zoomTargetPosition = cameraHolder.localPosition;
        eventSystem = EventSystem.current; // Get the EventSystem
    }

    private void Update()
    {
        Vector3 pos = transform.position;

        if (IsMouseOverUI()) // Check if the mouse is over UI elements
        {
            return; // Don't process camera movements
        }

        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        transform.position = pos;
        HandleInput();
        Rotation();
        Zoom();
        Move();
        AdjustCameraAngle();
    }

    private bool IsMouseOverUI()
    {
        return eventSystem.IsPointerOverGameObject();
    }

    private void Rotation()
    {
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * rotationSmoothing);
        transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.up);
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.E)) // Press "E" to rotate right
        {
            targetAngle += rotationSpeed * Time.deltaTime * rotationSmoothing;
        }
        else if (Input.GetKey(KeyCode.Q)) // Press "Q" to rotate left
        {
            targetAngle -= rotationSpeed * Time.deltaTime * rotationSmoothing;
        }

        zoomInput = Input.GetAxisRaw("Mouse ScrollWheel");

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 right = transform.right * x;
        Vector3 forward = transform.forward * z;

        input = (forward + right).normalized;
    }

    private void Move()
    {
        Vector3 nextTargetPosition = targetPosition + input * speed;
        if (IsInBounds(nextTargetPosition)) targetPosition = nextTargetPosition;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothing);
    }

    private void Zoom()
    {
        Vector3 nextZoomTargetPosition = zoomTargetPosition + cameraDirection * (zoomInput * zoomSpeed);
        if (IsInZoomBounds(nextZoomTargetPosition)) zoomTargetPosition = nextZoomTargetPosition;
        cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, zoomTargetPosition, Time.deltaTime * smoothing);
    }

    private void AdjustCameraAngle()
    {
        float zoomFactor = Mathf.InverseLerp(zoomRange.x, zoomRange.y, cameraHolder.localPosition.magnitude);
        float targetRotation = Mathf.Lerp(maxZoomAngle, minZoomAngle, zoomFactor);
        cameraHolder.localRotation = Quaternion.Euler(targetRotation, 0f, 0f);
    }

    private bool IsInBounds(Vector3 position)
    {
        return position.x > -range.x &&
            position.x < range.x &&
            position.z > -range.y &&
            position.z < range.y;
    }

    private bool IsInZoomBounds(Vector3 position)
    {
        return position.magnitude > zoomRange.x && position.magnitude < zoomRange.y;
    }
}
