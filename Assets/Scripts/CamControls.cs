using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamControls : MonoBehaviour
{
    [SerializeField]
    private Bounds bounds; // This variable was already here, so I'm assuming it's okay for me to use it the same way as we have been.

    [SerializeField]
    private float _zoomLevel; // The amount of zoom

    [SerializeField]
    private float _zoomMultiplier = 8f; // What the zoom will be multiplied by

    [SerializeField]
    private float _zoomMin = 2f; // Minimum zoom level

    [SerializeField]
    private float _zoomMax = 50f; // Maximum zoom level

    [SerializeField]
    private float _velocity = 0f; // Velocity (bet nobody could have figured that out without this comment in specific)

    [SerializeField]
    private float _smoothTime = 0.25f; // The amount of time in seconds it will take the camera to transition between the levels of zoom

    [SerializeField]
    private float _rotationSensitivity = 50f; // How sensitive the camera's rotation is

    [SerializeField]
    private float _yRotation = 0f; // Rotation around the Y axis

    [SerializeField]
    private float _xRotation = 0f; // Rotation around the X axis

    private Vector3 _cameraPosition; // Camera's current position

    [SerializeField]
    private float _panSpeed = 20f; // How fast the camera pans

    [SerializeField]
    private Camera _camera; // Reference to the camera

    [SerializeField]
    private Vector3 _maxAngle; // Furthest the camera will rotate when zooming

    [SerializeField]
    private Vector3 _minAngle; // Smallest amount the camera will rotate when zooming

    void Start()
    {
        // Sets the zoom level to the orthographic size of the camera when the game starts
        _zoomLevel = _camera.fieldOfView;

        // Sets the cam pivot's position variable to the current position
        _cameraPosition = this.transform.position;
    }

    // This stopped working after I added the part that changes the min and max angles with the zoom level. Changes whether the orbit works at all if Zoom() or Orbit() are called first. With Zoom() called first, orbitting works, but the angle immediately snaps back to 0 the moment I click. I'm pretty sure this is to do with interactions between the camera and its parent, but I'm running out of (reasonable) time and I'm going to have to call it here.
    private void Orbit()
    {
        // If we are holding down the right mouse button...
        if (Input.GetMouseButton(1))
        {
            // Rotates the camera around the Y axis when moving the mouse from side to side
            // Removed the X rotation from the tutorial I followed and changed "Mouse Y" to "Mouse X" so that the camera rotates when moving the mouse side to side, not up and down.
            _yRotation += Input.GetAxis("Mouse X") * Time.deltaTime * _rotationSensitivity;

            // This allows the cam pivot's transform to be rotated in local space (I think, from reading the Unity Docs)
            transform.localEulerAngles = new Vector3(_xRotation, _yRotation, 0);
        }

    }

    private void Pan()
    {
        // Move camera forwards when pressing W
        if (Input.GetKey(KeyCode.W))
        {
            _cameraPosition += transform.forward * _panSpeed * Time.deltaTime;
        }

        // Move camera backwards when pressing S
        if (Input.GetKey(KeyCode.S))
        {
            _cameraPosition -= _panSpeed * Time.deltaTime * transform.forward;
        }

        // Move camera left when pressing A
        if (Input.GetKey(KeyCode.A))
        {
            _cameraPosition -= transform.right * _panSpeed * Time.deltaTime;
        }

        // Move camera right when pressing D
        if (Input.GetKey(KeyCode.D))
        {
            _cameraPosition += _panSpeed * Time.deltaTime * transform.right;
        }

        // Updates the camera's position
        this.transform.position = _cameraPosition;

        // Keeps camera within boundaries
        transform.position = bounds.ClosestPoint(transform.position);
    }

    public void Zoom()
    {

        // If the camera is zoomed all the way out, set the rotation to the max angle.
        if (_zoomLevel == _zoomMax)
        {
            this.transform.rotation = Quaternion.Euler(_maxAngle);
        }
        // Else, set the rotation to the min angle.
        else
        {
            this.transform.rotation = Quaternion.Euler(_minAngle);
        }

        // References the scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Multiplies the input of the scroll wheel by the zoom multiplier and subtracts that result from the zoom level.
        // "The zoom multiplier is necessary so that we don't get tendonitis from scolling." - The guy whose tutorial I followed
        _zoomLevel -= scroll * _zoomMultiplier;

        // Clamps the zoom + angle between the min and max values so we don't zoom forever and ever and ever
        _zoomLevel = Mathf.Clamp(_zoomLevel, _zoomMin, _zoomMax);

        // Smooths the movement of the camera as it zooms in and out
        _camera.fieldOfView = Mathf.SmoothDamp(_camera.fieldOfView, _zoomLevel, ref _velocity, _smoothTime);

    }

    void Update()
    {
        // If the mouse is not hovered over a UI element:
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            Pan();
            Zoom();
            Orbit();
        }
    }
}