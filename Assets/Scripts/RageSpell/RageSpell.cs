using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageSpell : MonoBehaviour
{
    private Vector3 targetPosition;

    public void ResetToInitialState()
    {
        targetPosition = Vector3.zero;
    }

    public void Init(Vector3 targetPos)
    {
        targetPosition = targetPos;

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
