using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamControls : MonoBehaviour
{
    [SerializeField]
    float panSpeed = 10f;
    [SerializeField]
    float keyboardPanSpeed = 20f;
    [SerializeField]
    float scrollSpeed = 20f;
    [SerializeField]
    float rotationSpeed = 30f;
    [SerializeField]
    Collider boundingCollider;
    [SerializeField]
    float minZoom = 5f;
    [SerializeField]
    float maxZoom = 80f;
    [SerializeField]
    float padding = 2f;
    [SerializeField]
    float maxZoomTiltAngle = 30f;
    [SerializeField]
    float normalTiltAngle = 0f;
    [SerializeField]
    float tiltThreshold = 40f;

    private Camera mainCamera;
    private Vector3 lastMousePosition;
    private float edgePanThreshold = 10f;
    private float currentTiltAngle;

    void Start()
    {
        mainCamera = Camera.main;
        currentTiltAngle = normalTiltAngle;
    }

    void Update()
    {
        if (CanControlCamera())
        {
            PanWithEdge();
            PanWithKeyboard();
            RotateWithKeys();
            ZoomWithMouseScroll();
            TiltCamera();
            ClampCameraPosition();
            LimitZoom();
        }
    }

    bool CanControlCamera()
    {
        return !EventSystem.current.IsPointerOverGameObject();
    }

    void PanWithEdge()
    {
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;
        Vector3 delta = Vector3.zero;

        if (mouseX < edgePanThreshold)
        {
            delta += Vector3.left;
        }
        else if (mouseX > Screen.width - edgePanThreshold)
        {
            delta += Vector3.right;
        }
        if (mouseY < edgePanThreshold)
        {
            delta += Vector3.back;
        }
        else if (mouseY > Screen.height - edgePanThreshold)
        {
            delta += Vector3.forward;
        }
        delta.Normalize();
        delta *= panSpeed * Time.deltaTime;

        transform.Translate(delta);
    }

    void PanWithKeyboard()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 keyboardPan = new Vector3(horizontalInput, 0, verticalInput) * keyboardPanSpeed * Time.deltaTime;
        transform.Translate(keyboardPan);
    }

    void RotateWithKeys()
    {
        float rotation = 0f;
        if (Input.GetKey("e"))
        {
            rotation = rotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey("q"))
        {
            rotation = -rotationSpeed * Time.deltaTime;
        }
        transform.Rotate(Vector3.up, rotation);
    }
    void ZoomWithMouseScroll()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float newZoom = mainCamera.fieldOfView - scrollInput * scrollSpeed * Time.deltaTime;
        mainCamera.fieldOfView = Mathf.Clamp(newZoom, minZoom, maxZoom);
    }
    void TiltCamera()
    {
        float zoomFraction = (mainCamera.fieldOfView - minZoom) / (maxZoom - minZoom);
        float targetTilt = Mathf.Lerp(normalTiltAngle, maxZoomTiltAngle, zoomFraction);
        if (mainCamera.fieldOfView > tiltThreshold)
        {
            currentTiltAngle = Mathf.Lerp(currentTiltAngle, targetTilt, Time.deltaTime * 5f);
        }
        else
        {
            currentTiltAngle = Mathf.Lerp(currentTiltAngle, normalTiltAngle, Time.deltaTime * 5f);
        }
        mainCamera.transform.localRotation = Quaternion.Euler(currentTiltAngle, 0, 0);
    }

    void ClampCameraPosition()
    {
        if (boundingCollider != null)
        {
            Bounds bounds = boundingCollider.bounds;
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(transform.position.x, bounds.min.x + padding, bounds.max.x - padding);
            clampedPosition.z = Mathf.Clamp(transform.position.z, bounds.min.z + padding, bounds.max.z - padding);
            transform.position = clampedPosition;
        }
    }
    void LimitZoom()
    {
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, minZoom, maxZoom);
    }
}