using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControls : MonoBehaviour
{
    [SerializeField]
    private Bounds bounds;
    [SerializeField] private int _panningSpeed = 10;
    [SerializeField] private int _zoomSpeed = 10;
    [SerializeField] private float _rotateSpeed = 75f;
    [SerializeField] private int _maxZoom = 5;
    [SerializeField] private int _minZoom = 100;
    [SerializeField] private int _defaultFOV = 60;
    [SerializeField] private float _maxXRotation = 15;


    private Camera _mainCamera;
   
    private float _rotationIncrement; 

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        _mainCamera.fieldOfView = _defaultFOV;
        _rotationIncrement = _maxXRotation / (_minZoom - _defaultFOV);
    }

    // Update is called once per frame
    void Update()
    {
        //pans the camera based on the horizontal and vertical axis which are assigned to WASD and the arrow keys
        float verticalTranslation = (Input.GetAxis("Vertical") * _panningSpeed) * Time.deltaTime;
        float horizontalTranslation = (Input.GetAxis("Horizontal") * _panningSpeed) * Time.deltaTime;
        transform.Translate(horizontalTranslation, 0, verticalTranslation);
        transform.position = bounds.ClosestPoint(transform.position); //keeps the camera in bounds

        //zooms the camera in/out when using the scroll wheel 
        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if(_mainCamera.fieldOfView <= _maxZoom && Input.GetAxis("Mouse ScrollWheel") > 0) //limits how far it can zoom in
            {
                return;
            }
            else if(_mainCamera.fieldOfView >= _minZoom && Input.GetAxis("Mouse ScrollWheel") < 0) //limits how far it can zoom out
            {
                return;
            }
            else //zooms the camera in or out and adjusts the cameras rotation accordingly
            {   
                _mainCamera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed; //zooms the camera in or out
                
                if(_mainCamera.fieldOfView > _defaultFOV && transform.localRotation.eulerAngles.x < _maxXRotation && Input.GetAxis("Mouse ScrollWheel") < 0) //rotates camera downwards if zoomed out
                {
                   transform.Rotate(new Vector3(_rotationIncrement * -Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed, 0, 0), Space.Self);                    
                }

                else if(_mainCamera.fieldOfView > _defaultFOV && Input.GetAxis("Mouse ScrollWheel") > 0) //rotates camera upwards if zoomed in
                {
                    transform.Rotate(new Vector3(-_rotationIncrement * Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed, 0, 0), Space.Self); 
                }

                else if(_mainCamera.fieldOfView < _defaultFOV && transform.localRotation.eulerAngles.x != 0) //sets x rotation to 0 if not already
                {
                    transform.Rotate(new Vector3(-transform.localRotation.eulerAngles.x,0,0));
                }
            }
           
        }


        if(Input.GetMouseButton(2)) //rotates camera around the world
        {
            Input.GetAxis("Mouse X");
            transform.RotateAround(Vector3.zero, new Vector3( 0, Input.GetAxis("Mouse X"), 0) , _rotateSpeed * Time.deltaTime);
        }
    }
}
