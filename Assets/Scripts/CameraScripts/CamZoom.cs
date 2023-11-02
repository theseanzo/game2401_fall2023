using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamZoom : MonoBehaviour
{
    #region Variables Declaration
    [SerializeField]
    private float _zoomRangeMin = 10f;
    [SerializeField]
    private float _zoomRangeMax = 30f;
    [SerializeField]
    private float _zoomSpeed = 10f;
    [SerializeField]
    private float rotationSpeed = 50.0f;
    [SerializeField]
    private Camera _orthographicCamera;
    [SerializeField]
    private float _minRotation = 43.148f;
    [SerializeField]
    private float _maxRotation = 55f;
    [SerializeField]
    private float _currentRotationX;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _orthographicCamera = GetComponent<Camera>();
    if (_orthographicCamera != null) //checking if there are any camera at the scene
        {
            float cameraSize = _orthographicCamera.orthographicSize;
        }
        else
        {
            Debug.LogError("The camera was not found");
        }
    }

    // Update is called once per frame
    void Update()
    {

        _currentRotationX = transform.rotation.eulerAngles.x;
        float input = Input.GetAxis("Mouse ScrollWheel"); //This same input will be used to zoom and to rotate the camera as a eagle view
        float newCameraSize = _orthographicCamera.orthographicSize - input * _zoomSpeed;
        newCameraSize = Mathf.Clamp(newCameraSize, _zoomRangeMin, _zoomRangeMax);
        _orthographicCamera.orthographicSize = newCameraSize;
        if (_currentRotationX < _maxRotation && _currentRotationX > _minRotation && newCameraSize < _zoomRangeMax && newCameraSize > _zoomRangeMin) //Alligning the limitations evolved in the rotation movement
        transform.Rotate(Vector3.right, input * rotationSpeed); 
    }
}
