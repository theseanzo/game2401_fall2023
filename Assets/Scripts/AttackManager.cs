using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : Singleton<AttackManager>
{
    public Unit currentPrefab;//this will be the unit we are currently placing
    // Start is called before the first frame update
    private void Awake()
    {
        gameObject.SetActive(GameManager.Instance.CurrentState == GameState.Attacking);
    }
    void Start()
    {
        
    }
    public void SetCurrent(Unit unitPrefab)
    {
        currentPrefab = unitPrefab;  //this will set up whatever unit we are going to be placing
    }
    // Update is called once per frame
    void Update() //here we are going to do all of the magic we did before except not base ont the grid
    {
        //let's make sure that we have a current prefab to place as well as we are clicking down
        if(currentPrefab != null && Input.GetMouseButtonDown(0)) //make sure we use the left mouse button
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); //let's get the ray from the camera to cast into the scene
            RaycastHit hitInfo;
            if(Physics.Raycast(mouseRay, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Terrain")))
            {
                Vector3 pos = hitInfo.point;
                Instantiate(currentPrefab, pos, Quaternion.identity); //create a new unit where the terrain is hit
                //recall that this succeeds only if we actually hit the terrain and the information about the hit is in the hitInfo
            }
        }
    }
}
