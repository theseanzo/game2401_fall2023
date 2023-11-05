using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;//Added for UI and rotation control
public class CameraMotion : MonoBehaviour  ////Script attached into main camera
{

    [SerializeField]
    private float Speed = 20f;
    [SerializeField]
    private Vector2 panLimit; //X and Y value



    [SerializeField]
    private float scrollSpeed = 20f; //Scroll speed and position information is given here.
    [SerializeField]
    private float minY = 20f;
    [SerializeField]
    private float maxY = 140f;



    [SerializeField]
    private float rotationSpeed = 3.0f;//Rotation and movement speed are determined.

    public float smooth = 5; // Smooth camera control

    private float rotationX = 0.0f;
    // private float rotSpeed;

    private bool isCursorOverUI = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the EventSystem if it's not already in the scene.
        if (EventSystem.current == null)
        {
            new GameObject("EventSystem", typeof(EventSystem));
        }

    }

    // Update is called once per frame
    void Update()
    {

        Vector3 pos = transform.position;

        isCursorOverUI = EventSystem.current.IsPointerOverGameObject();//define cursor


        
        if (!isCursorOverUI) //If cursor on the UI camera control and keyboard control will be diactive.
        {

            //Two different code systems were defined for mouse controls.
            //The active code stack is directed with the keyboard keys. The inactive one is for mouse control.
            if (Input.GetKey("w"))// || Input.mousePosition.y >= Screen.height - thickness)
            {
                pos.z += Speed * Time.deltaTime;
            }

            if (Input.GetKey("s"))// || Input.mousePosition.y <= Screen.height - thickness)
            {
                pos.z -= Speed * Time.deltaTime;
            }
            if (Input.GetKey("d"))// || Input.mousePosition.x >= Screen.width - thickness)
            {
                pos.x += Speed * Time.deltaTime;
            }
            if (Input.GetKey("a"))// || Input.mousePosition.x <= Screen.width - thickness)
            {
                pos.x -= Speed * Time.deltaTime;
            }



            float scroll = Input.GetAxis("Mouse ScrollWheel"); //Getting mouse scroll information. Adjusts scrolling movements in X,Y,Z
            pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime / smooth;
            pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);// lock the limits during movement
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);
            transform.position = pos;


            float mouseX = Input.GetAxis("Mouse X"); //Getting mouse X an Y informations.
            float mouseY = Input.GetAxis("Mouse Y");
            rotationX -= mouseY * rotationSpeed;
            rotationX = Mathf.Clamp(rotationX, -90, 90); //On X position up to down

            transform.localRotation = Quaternion.Euler(rotationX, 0, 0); // For 360 degree rotation around the camera 
            transform.parent.Rotate(Vector3.up * mouseX * rotationSpeed / smooth);
        }
    }
}