using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : Building
{
    [SerializeField]
    private Transform waterSplashOrigin; 
    private WaterSplashDefense waterSplashDefense; 

    private void Start()
    {
        base.Start();
        InitializeWaterSplashDefense();
    }

    private void InitializeWaterSplashDefense()
    {
        waterSplashDefense = gameObject.AddComponent<WaterSplashDefense>();
        waterSplashDefense.enabled = false; 
    }

    public override void OnDie()
    {
        base.OnDie();
        Destroy(waterSplashDefense);
    }
}
