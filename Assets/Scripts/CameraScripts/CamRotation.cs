using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotation : MonoBehaviour
{
    #region Variables Declaration
    public Transform cameraPivot;
    public float _speed = 10.0f;
    #endregion



    // Update is called once per frame
    void Update()
    {
        float inputRotation = Input.GetAxis("CameraRotation");//According with the player input with the mouse scroll wheel
        transform.RotateAround(cameraPivot.position, Vector3.up, inputRotation * _speed * Time.deltaTime);//The camera will just rotate around the [CameraPivot] position
    }
}
