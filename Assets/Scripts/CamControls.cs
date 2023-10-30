using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControls : MonoBehaviour
{
    // Publicly Tweakable in Inspector
    [SerializeField] private float _minZoom = 5.0f; // Minimum zoom
    public float minZoom
    {
        get { return _minZoom; }
        set { _minZoom = value; }
    }

    [SerializeField] private float _maxZoom = 100.0f; // Maximum zoom
    public float maxZoom
    {
        get { return _maxZoom; }
        set { _maxZoom = value; }
    }

    [SerializeField] private float _zoomSpeed = 5.0f; // Zoom speed
    public float zoomSpeed
    {
        get { return _zoomSpeed; }
        set { _zoomSpeed = value; }
    }

    [SerializeField] private float _moveSpeed = 5.0f; // Movement speed
    public float moveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }

    [SerializeField] private float _rotationSpeed = 5.0f; // Rotation speed
    public float rotationSpeed
    {
        get { return _rotationSpeed; }
        set { _rotationSpeed = value; }
    }

    [SerializeField] private bool _disableCameraOverUI = true; // Flag to disable camera control
    public bool disableCameraOverUI
    {
        get { return _disableCameraOverUI; }
        set { _disableCameraOverUI = value; }
    }


    private float currentZoom;//current zoom 
    private float minBirdsEyeAngle = 30.0f; // Minimum bird's eye view angle
    private float maxBirdsEyeAngle = 80.0f; // Maximum bird's eye view angle
    private bool isCameraMovementEnabled = true;//flag to control camera moverment


    private void Start()
    {
        currentZoom = Camera.main.fieldOfView;//initialize current zoom to the camera's field of view
    }

    private void Update()
    {
        if (disableCameraOverUI && isCameraMovementEnabled)//check ui elements
        {
            HandleZoom();
            HandleRotation();
            HandleMovement();
            AdjustBirdsEyeAngle();
        }
    }

    //Zoom in and out and Google Maps-like Zoom 
    private void HandleZoom()
    {
        float zoomAmount = -Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;//get the mouse scroll wheel input
        currentZoom = Mathf.Clamp(currentZoom + zoomAmount, minZoom, maxZoom);// Clamp the zoom level
        Camera.main.fieldOfView = currentZoom;//// Update the camera's field of view

        //get the mouse position as a screen point
        Vector3 mouseScreenPosition = Input.mousePosition;

        //convert the screen point to a ray
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);

        //calculate the zoom direction
        Vector3 zoomDirection = ray.direction * zoomAmount;

        //update the camera position
        transform.position += zoomDirection;
    }


    //New Panning-use keyboard "w,a,s,d"
    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");//get horizontal input
        float verticalInput = Input.GetAxis("Vertical");//get vertical input

        //calculate movement direction
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        //translate the camera
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }


    //360 Degree Rotation
    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");//get mouse X-axis input
        Vector3 currentEulerAngles = transform.eulerAngles;//calculate rotation angle
        currentEulerAngles.y += mouseX;
        transform.eulerAngles = currentEulerAngles;
    }

    //Camera Bird’s Eye View Angle
    private void AdjustBirdsEyeAngle()
    {
        // Calculate the normalized zoom level (0 to 1)
        float normalizedZoom = Mathf.InverseLerp(minZoom, maxZoom, currentZoom);

        // Adjust the camera's field of view based on the zoom level
        Camera.main.fieldOfView = Mathf.Lerp(minBirdsEyeAngle, maxBirdsEyeAngle, normalizedZoom);
    }


    //Disable Camera Over UI ,Use EvenTrigger directly to UI
    public void OnMouseOverUI()
    {
        isCameraMovementEnabled = false;
    }

    public void OnMouseExitUI()
    {
        isCameraMovementEnabled = true;
    }
}
