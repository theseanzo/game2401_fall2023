using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamControls : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [SerializeField] private float zoomMin = 30.0f;
    [SerializeField] private float zoomMax = 70.0f;
    [SerializeField] private float zoomAmount = 100.0f;

    [SerializeField] private float camAngleMax = 70.0f;
    [SerializeField] private float camAngleMin = 20.0f ;

    [SerializeField] private float moveSpeed = 75.0f;
    [SerializeField] private float rotateSpeed = 50.0f;

    private Vector3 mousePos;

    #region Camera Functions
    private void CameraZoom()
    {
        float zoom = Input.GetAxis("Mouse ScrollWheel");
        if (zoom != 0)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//get the mouse position in world space
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - zoom * zoomAmount * Time.deltaTime, zoomMin, zoomMax);//change the orthographic size of the camera based on the mouse scrollwheel

            float zoomLevel = (cam.orthographicSize - zoomMin) / (zoomMax - zoomMin); //calculate zoomlevel between 0 and 1
            float camAngle = Mathf.Lerp(camAngleMin, camAngleMax, zoomLevel);//get the angle between the min and max angle based on the zoomlevel
            cam.transform.rotation = Quaternion.Euler(new Vector3(camAngle, transform.eulerAngles.y, 0)); //set the rotation of the camera to the new angle


            Vector3 newMousePos = mousePos - Camera.main.ScreenToWorldPoint(Input.mousePosition);//get the difference between the mouse position and the camera's position
            transform.position += newMousePos;//move the camera in the direction of the difference
        }
    }
    private void CameraMove()//move the camera parent in the direction forward and right vectors
    {
        if (Input.GetKey(KeyCode.W))
            transform.position += moveSpeed * Time.deltaTime * transform.forward;
        if (Input.GetKey(KeyCode.S))
            transform.position -= moveSpeed * Time.deltaTime * transform.forward;
        if (Input.GetKey(KeyCode.A))
            transform.position -= moveSpeed * Time.deltaTime * transform.right;
        if (Input.GetKey(KeyCode.D))
            transform.position += moveSpeed * Time.deltaTime * transform.right;
    }
    private void CameraRotate()//rotate the camera around the y axis
    {
        if (Input.GetKey(KeyCode.E))
            transform.RotateAround(transform.position, Vector3.up, rotateSpeed * Time.deltaTime); //rotate around the y axis
        if (Input.GetKey(KeyCode.Q))
            transform.RotateAround(transform.position, Vector3.up, -rotateSpeed * Time.deltaTime); //rotate around the y axis but -rotateSpeed instead
    }
    #endregion
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())//check if the mouse is over a UI element
        {
            CameraZoom();
            CameraRotate();
            CameraMove();
        }
    }
}
