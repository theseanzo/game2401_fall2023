using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControl : MonoBehaviour
{
    public float panBorderThickness = 2f;
    public float panSpeed = 20f;
    public Vector2 panLimit;

    [SerializeField]
    private float zoomScale = 10f;
    [SerializeField]
    private float minZoom = 30f;
    [SerializeField]
    private float maxZoom = 90f;
    [SerializeField]
    private float rotationSpeed = 500f;
    private Vector3 mouseWorldPosStart;

    // Update is called once per frame
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButton(2))
            {
                CamOrbit();
            }

            Zoom(Input.GetAxis("Mouse ScrollWheel"));

            Vector3 pos = transform.position;
            if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panBorderThickness)
            {
                pos.z += panSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorderThickness)
            {
                pos.z -= panSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorderThickness)
            {
                pos.x += panSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorderThickness)
            {
                pos.x -= panSpeed * Time.deltaTime;
            }
            pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
            pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

            transform.position = pos;

        }
    }
    private void CamOrbit()
    {
        if(Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            float verticalInput = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.right, -verticalInput);
            transform.Rotate(Vector3.up, horizontalInput, Space.World);
        }
    }
    private void Zoom(float zoomDiff)
    {
        if(zoomDiff != 0)
        {
            mouseWorldPosStart = GetPerspectivePos();
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - zoomDiff * zoomScale, minZoom, maxZoom);
            Vector3 mouseWorldPosDiff = mouseWorldPosStart - GetPerspectivePos();
            transform.position += mouseWorldPosDiff;
        }
    }
    public Vector3 GetPerspectivePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(transform.forward, 0.0f);
        float dist;
        plane.Raycast(ray, out dist);
        return ray.GetPoint(dist);
    }
}
