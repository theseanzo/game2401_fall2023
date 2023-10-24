using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControls : MonoBehaviour
{
    //[SerializeField]
    //private Bounds bounds;

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
    private Camera _camera; // Reference to the camera

    void Start()
    {
        // Sets the zoom level to the orthographic size of the camera when the game starts
        _zoomLevel = _camera.fieldOfView;
    }

    void Update()
    {
        // References the scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Multiplies the input of the scroll wheel by the zoom multiplier and subtracts that result from the zoom level.
        // "The zoom multiplier is necessary so that we don't get tendonitis from scolling." - The guy whose tutorial I followed
        _zoomLevel -= scroll * _zoomMultiplier;

        // Clamps the zoom between the min and max values so we don't zoom forever and ever and ever
        _zoomLevel = Mathf.Clamp(_zoomLevel, _zoomMin, _zoomMax);

        // Smooths the movement of the camera as it zooms in and out
        _camera.fieldOfView = Mathf.SmoothDamp(_camera.fieldOfView, _zoomLevel, ref _velocity, _smoothTime);





        // These are the old camera controls. I will be making new ones, using these as reference. - Shaylyn

        ////we are going to simply move according to our mouse movement if we hold down the left mouse button
        //if (Input.GetMouseButton(1)) //question: what is the difference between GetMouseButton and GetMouseButtonDown?
        //{
        //    float mouseXMovement = Input.GetAxis("Mouse X");
        //    float mouseYMovement = Input.GetAxis("Mouse Y");
        //    this.transform.Translate(-mouseXMovement, 0, -mouseYMovement);
        //    transform.position = bounds.ClosestPoint(transform.position);
        //}
    }
}
