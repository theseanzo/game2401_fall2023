 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControls : MonoBehaviour
{
    [SerializeField]
    private Bounds bounds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //we are going to simply move according to our mouse movement if we hold down the left mouse button
        if (Input.GetMouseButton(1)) //question: what is the difference between GetMouseButton and GetMouseButtonDown?
        {
            float mouseXMovement = Input.GetAxis("Mouse X");
            float mouseYMovement = Input.GetAxis("Mouse Y");
            this.transform.Translate(-mouseXMovement, 0, -mouseYMovement);
            transform.position = bounds.ClosestPoint(transform.position);
        }
    }
}
