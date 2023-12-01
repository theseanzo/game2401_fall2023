using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellObject : MonoBehaviour
{
    private Vector3 targetPosition;//targetPosition;

    public void ResetToInitialState()
    {
        targetPosition = Vector3.zero;
    }

    public void Init(Vector3 targetPos)
    {
        targetPosition = targetPos;// set the target position for the spell

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SomeMethod()
    {
        gameObject.SetActive(false);
    }
}
