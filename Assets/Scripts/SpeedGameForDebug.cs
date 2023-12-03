using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedGameForDebug : MonoBehaviour
{
    public int Timescale;
    
    void OnEnable()
    {
        Time.timeScale = Timescale;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
