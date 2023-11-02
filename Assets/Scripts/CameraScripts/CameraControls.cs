using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    #region Variables Declaration
    [SerializeField]
    private float _speed = 15.0f;
    public Transform cameraPivot;
    #endregion


    // Update is called once per frame
    void Update()
    {
        //Getting the inputs
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalMovement, verticalMovement, 0);

        if (movement.magnitude > 2) 
        {
            movement.Normalize(); //just to stabilize the speed of movement
        }


        transform.Translate(movement * _speed * Time.deltaTime);

        if (cameraPivot != null)
        {
            //The idea here is to keep the camera pivot moving, always in the middle of the camera's perspective, regardless of the movement it makes.
            Vector3 newPivotPosition = cameraPivot.position;
            newPivotPosition.x = transform.position.x;
            newPivotPosition.y = transform.position.y;
            cameraPivot.position = newPivotPosition;
        }
    }
}
