using UnityEngine;
using UnityEngine.EventSystems;

public class CamControls : MonoBehaviour
{
    #region Inspector Variables
    [Header("Zoom Settings")]
    [SerializeField] private float maxZoom = 15f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float zoomScale = 1f;

    [Header("Panning Settings")]
    [SerializeField] private float panningSpeed = 20f;
    [SerializeField] private int edgeScrollSize = 10;

    [Header("Rotation Settings")]
    [SerializeField] private float rotateSpeed = 100f;

    [Header("Camera Angle Settings")]
    [SerializeField] private float zoomViewAngleSpeed = 3f;
    [SerializeField] private float minViewAngle = 45f;
    [SerializeField] private float maxViewAngle = 90f;

    [Header("Movement Bounds")]
    [SerializeField] private Bounds bounds;
    #endregion

    private Camera mainCamera;
    private Vector3 mouseWorldPos;

    #region Unity Methods
    void Start()
    {
        mainCamera = GetComponentInChildren<Camera>(); // Get the main camera
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; // Ignore UI

        Panning(); // Move with keyboard and mouse
        Zoom(Input.GetAxis("Mouse ScrollWheel")); // Zoom with mouse wheel
        Rotate(); // Rotate with Q and E
    }
    #endregion

    #region Camera Movement Methods
    private void Panning()
    {
        // Get input direction
        Vector3 inputDir = Vector3.zero;

        // Panning with Keyboard
        if (Input.GetKey(KeyCode.W)) inputDir.z += 1f;
        if (Input.GetKey(KeyCode.S)) inputDir.z -= 1f;
        if (Input.GetKey(KeyCode.D)) inputDir.x += 1f;
        if (Input.GetKey(KeyCode.A)) inputDir.x -= 1f;

        // Edge Scrolling
        HandleEdgeScrolling(ref inputDir);

        // Translate camera with speed adjusted for framerate independence
        this.transform.Translate(inputDir * panningSpeed * Time.deltaTime);
        transform.position = bounds.ClosestPoint(transform.position);
    }

    private void HandleEdgeScrolling(ref Vector3 inputDir)
    {
        // Check if mouse position is at the setup edge
        if (Input.mousePosition.x < edgeScrollSize) inputDir.x -= 1f;
        if (Input.mousePosition.y < edgeScrollSize) inputDir.z -= 1f;
        if (Input.mousePosition.x > Screen.width - edgeScrollSize) inputDir.x += 1f;
        if (Input.mousePosition.y > Screen.height - edgeScrollSize) inputDir.z += 1f;
    }

    private void Zoom(float zoomDiff)
    {
        if (zoomDiff != 0) // Zoom with mouse wheel
        {
            float zoomLevel = mainCamera.orthographicSize - zoomDiff * zoomScale;
            mainCamera.orthographicSize = Mathf.Clamp(zoomLevel, minZoom, maxZoom);

            mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mouseWorldPosDiff = mouseWorldPos - mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position += mouseWorldPosDiff;

            AdjustViewAngle(zoomLevel);
        }
    }

    private void AdjustViewAngle(float zoomLevel)
    {
        // Adjust the camera's angle based on zoom level
        float angle = Mathf.Lerp(maxViewAngle, minViewAngle, (zoomLevel - minZoom) / (maxZoom - minZoom));
        transform.rotation = Quaternion.Euler(new Vector3(angle, transform.eulerAngles.y, 0));
    }

    private void Rotate()
    {
        // Rotate with Q and E
        float rotateDir = Input.GetKey(KeyCode.E) ? 1f : Input.GetKey(KeyCode.Q) ? -1f : 0f;
        transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
    }
    #endregion
}
