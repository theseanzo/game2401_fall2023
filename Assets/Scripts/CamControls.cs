using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControls : MonoBehaviour
{
    public Camera mainCamera;

    
    [SerializeField] public float sensitivity;
    [SerializeField] public float maxFOV;
    [SerializeField] public float minFOV;
    [SerializeField] public float FOV;

    [SerializeField] public float panSpeed = 20f;
    [SerializeField]public float panBorderThickness = 10f;

    [SerializeField] public float rotationSpeed = 5f;

   
    [SerializeField]  float minZoom = 5f;
    [SerializeField]  float maxZoom = 20f;
    [SerializeField]  float minAngle = 45f;

    [SerializeField] private float maxAngle = 90f;
    

[SerializeField]
    private Bounds bounds;
    // Start is called before the first frame update

    void Start()
    {
        
        mainCamera = GetComponent<Camera>();
    }
    private void UpdateCameraAngle()
    {
        // Calculate the zoom level based on the camera's field of view.
        float zoomLevel = Mathf.InverseLerp(minZoom, maxZoom, mainCamera.fieldOfView);

        // Calculate the angle based on the zoom level.
        float angle = Mathf.Lerp(minAngle, maxAngle, zoomLevel);

        // Set the camera's rotation to the calculated angle.
        mainCamera.transform.rotation = Quaternion.Euler(angle, mainCamera.transform.rotation.eulerAngles.y, mainCamera.transform.rotation.eulerAngles.z);
    }




    // Update is called once per frame
    void Update()
    {
        UpdateCameraAngle();

        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            // Disable camera movements.
            mainCamera.enabled = false;
        }
        else
        {
            // Enable camera movements.
            mainCamera.enabled = true;
        }

        FOV = Camera.main.fieldOfView;
        FOV += (Input.GetAxis("Mouse ScrollWheel") * sensitivity) * -1;
        FOV = Mathf.Clamp(FOV, minFOV, maxFOV);
        Camera.main.fieldOfView = FOV;


        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * panSpeed * Time.deltaTime, 0, -touchDeltaPosition.y * panSpeed * Time.deltaTime);
        }

        // Move camera with keyboard input
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }

        float rotationInput = Input.GetAxis("Horizontal");

        float rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;

        transform.Rotate(0f, rotationAmount, 0f);



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
