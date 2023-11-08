using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float minZoomLevel = 5f;
    public float maxZoomLevel = 30f;
    public float panSpeed = 20f;
    public float rotationSpeed = 100f;
    public float minBirdsEyeAngle = 10f;
    public float maxBirdsEyeAngle = 70f;

    private bool _isCursorOverUI = false;

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        //check if cursor is over UI
        _isCursorOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

        if (!_isCursorOverUI)
        {
            HandleZoom();
            HandlePan();
            HandleRotation();
            HandleBirdsEye();
        }
    }

    private void HandleZoom()
    {
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
        float newZoomLevel = Mathf.Clamp(transform.position.y - zoomDelta * panSpeed, minZoomLevel, maxZoomLevel);

        if (newZoomLevel >= minZoomLevel && newZoomLevel <= maxZoomLevel)
        {
            //get the mouse position in screen coordinates
            Vector3 mouseScreenPos = Input.mousePosition;

            //convert the mouse screen position to a point in world
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, transform.position.y));

            Vector3 offset = transform.position - mouseWorldPos;
            offset *= 1 - zoomDelta;

            Vector3 newPosition = new Vector3(mouseWorldPos.x + offset.x, newZoomLevel, mouseWorldPos.z + offset.z);
            transform.position = newPosition;
        }
    }



    private void HandlePan()
    {
        float panX = 0f;
        float panZ = 0f;

        //check if the mouse is near the screen edges
        Vector3 mousePosition = Input.mousePosition;
        float edgeThickness = 20f; //how close to the edge the mouse needs to be for panning
        Rect screenRect = new Rect(edgeThickness, edgeThickness, Screen.width - 2 * edgeThickness, Screen.height - 2 * edgeThickness);

        if (screenRect.Contains(mousePosition))
        {
            //mouse is not near the screen edges, use WASD for movement
            panX = Input.GetAxis("Horizontal");
            panZ = Input.GetAxis("Vertical");
        }
        else
        {
            //mouse is near the screen edges, use mouse movement for panning
            panX = (mousePosition.x < edgeThickness) ? -1f : (mousePosition.x > Screen.width - edgeThickness) ? 1f : 0f;
            panZ = (mousePosition.y < edgeThickness) ? -1f : (mousePosition.y > Screen.height - edgeThickness) ? 1f : 0f;
        }

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 panDirection = (forward * panZ + right * panX).normalized;
        transform.Translate(panDirection * panSpeed * Time.deltaTime, Space.World);
    }




    private void HandleRotation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    private void HandleBirdsEye()
    {
        float range = Mathf.InverseLerp(minZoomLevel, maxZoomLevel, transform.position.y);
        float angle = Mathf.Lerp(minBirdsEyeAngle, maxBirdsEyeAngle, range);
        transform.rotation = Quaternion.Euler(angle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}


