using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeSpin : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 500;

    void Update()
    {
        transform.Rotate(0f, _rotationSpeed * Time.deltaTime, 0f, Space.Self); // Spins the object containing the cone army (in relation to itself. I'm not sure if I need Space.Self considering this object is a prefab set close to 0,0,0, but it doesn't seem to be hurting anything, either.)
    }
}
