using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smelin_Cone : Building
{
    [SerializeField]
    private Transform _theCones;

    private void Awake()
    {
        _theCones = transform.Find("TheCones"); // Finds the child object containing The Cones
    }

    void Update()
    {
        
    }
}
