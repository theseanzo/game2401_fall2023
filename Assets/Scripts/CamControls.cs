using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamControls : MonoBehaviour
{
    public float panSpeed = 5f;  // Speed of panning
    public float scrollSpeed = 10f;  // Speed of zooming
    public Collider2D boundsCollider;  // Collider for camera boundaries
    public float maxZoom = 5f; // Maximum zoom level
    public float minZoom = 15f; // Minimum zoom level
    public float padding = 1f; // Padding for the bounding box
    public float rotationSpeed = 30f; // Speed of rotation around Y-axis

    private Camera mainCamera;
    private bool isUIHovered = false;
    private Vector3 lastMousePosition;

    private float minX, maxX, minY, maxY;

    void Start()
    {
        mainCamera = Camera.main;



        if (boundsCollider != null)
        {
            Bounds bounds = boundsCollider.bounds;
            minX = bounds.min.x + padding;
            maxX = bounds.max.x - padding;
            minY = bounds.min.y + padding;
            maxY = bounds.max.y - padding;
        }
    }

    void Update()
    {

        if (EventSystem.current.IsPointerOverGameObject()) return;

        PanCameraWithArrowKeys();
        ZoomCameraWithMouseScroll();
        RotateCameraWithKeys();

    }


    void PanCameraWithArrowKeys() // Pan with WASD, arrow keys
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 keyboardPan = new Vector3(horizontalInput, 0, verticalInput) * panSpeed * Time.deltaTime;
        transform.Translate(keyboardPan);
    }

    void ZoomCameraWithMouseScroll() // Zoom with scroll
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float newZoom = mainCamera.fieldOfView - scrollInput * scrollSpeed * Time.deltaTime;
        mainCamera.fieldOfView = Mathf.Clamp(newZoom, minZoom, maxZoom);
    }

    void RotateCameraWithKeys()
    {
        if (Input.GetKey("e")) // Rotate clockwise
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey("q")) // Rotate counter-clockwise
        {
            transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }

    public void SetUIHovered(bool isHovered)
    {
        isUIHovered = isHovered;
    }


}