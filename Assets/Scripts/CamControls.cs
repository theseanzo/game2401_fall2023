using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamControls : MonoBehaviour
{
    [SerializeField]
    private Bounds bounds;

    [SerializeField]
    private float maxZoom;
    [SerializeField]
    private float minZoom;
    [SerializeField]
    private float zoomScale;

    [SerializeField]
    private int edgeScrollSize;


    [SerializeField]
    private float rotateSpeed;

    [SerializeField]
    private float zoomViewAngleSpeed;
    [SerializeField]
    private float minViewAngle;
    [SerializeField]
    private float maxViewAngle;


    private Camera mainCamera;

    private Vector3 mouseWorldPos;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!EventSystem.current.IsPointerOverGameObject()) //Check if mouse pointer is over the game or UI, if we are on the game, continue
        {
            Vector3 inputDir = new Vector3(0, 0, 0);

            if (Input.GetKey(KeyCode.W))
                inputDir.z = 1f;
            if (Input.GetKey(KeyCode.S))
                inputDir.z = -1f;
            if (Input.GetKey(KeyCode.D))
                inputDir.x = 1f;
            if (Input.GetKey(KeyCode.A))
                inputDir.x = -1f;

            //Edge Scrolling, it is done but I didn't like the effect of it that much
            if (Input.mousePosition.x < edgeScrollSize) //Check if mouse position on x is at the setup edge
            {
                inputDir.x = -1f;
            }
            if (Input.mousePosition.y < edgeScrollSize) // Check if mouse position on y is at the setup edge
            {
                inputDir.z = -1f;
            }
            if (Input.mousePosition.x > Screen.width - edgeScrollSize) // Check if mouse position on x is at the setup edge on the other side
            {
                inputDir.x = +1f;
            }
            if (Input.mousePosition.y > Screen.height - edgeScrollSize) // Check if mouse position on y is at the setup edge on the other side
            {
                inputDir.z = +1f;
            }

            this.transform.Translate(inputDir.x, 0, inputDir.z);
            transform.position = bounds.ClosestPoint(transform.position); // Sets the boundary of camera movement


/*            if (Input.GetMouseButton(1))
            {
                float mouseXMovement = Input.GetAxis("Mouse X");
                float mouseYMovement = Input.GetAxis("Mouse Y");
                this.transform.Translate(-mouseXMovement, 0, -mouseYMovement);
                transform.position = bounds.ClosestPoint(transform.position);
            }
            //we are going to simply move according to our mouse movement if we hold down the left mouse button
*/

            Zoom(Input.GetAxis("Mouse ScrollWheel")); //Zoom function

            Rotate(); //Rotate function

        }
    }

    private void Zoom(float zoomDiff)
    {
        if(zoomDiff != 0)
        {
            mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Get mouse world position
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - zoomDiff * zoomScale, minZoom, maxZoom); //Adjust camera orthograpic size to focus at mouse position
            Vector3 mouseWorldPosDiff = mouseWorldPos - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position += mouseWorldPosDiff; //Adjust position of camera to focus on mouse position

            float newAngle = Mathf.Clamp(transform.eulerAngles.x + zoomDiff * zoomViewAngleSpeed, minViewAngle, maxViewAngle); //set the new angle when zooming in
            transform.rotation = Quaternion.Euler(new Vector3(newAngle, transform.eulerAngles.y, 0));//rotate to new angle

        }
    }

    private void Rotate()
    {
        float rotateDir = 0f; //Set rotate direction
        if(Input.GetKey(KeyCode.E))
        {
            rotateDir = 1f;//Set a different direction depending on Input
        }
        if (Input.GetKey(KeyCode.Q))
        {
            rotateDir = -1f;
        }

        transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime,0); //Adjust rotation of the transform

    }

}
