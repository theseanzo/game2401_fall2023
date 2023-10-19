using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Our building manager is going to do the job of handling our buildings being added to our scene as well as being moved in the scene
public class BuildingManager : Singleton<BuildingManager>
{
    public Building current; //current building
    public Renderer grid; //the grid that we are working with

    [SerializeField]
    UnityEvent buildingAdded;
    //some private variables for the grid:
    [SerializeField]
    float gridFadeSpeed; //how long it takes for a grid to appear and disappear
    float gridAlpha; //this will be the transparency of the grid
    // Start is called before the first frame update
    void Start()
    {

    }
    private void Awake()
    {
        gameObject.SetActive(GameManager.Instance.CurrentState == GameState.Building);
    }

    // Update is called once per frame
    void Update()
    {
        //when we click on an object, what are the steps to see if we hit something?
        //let's go through that step first to see if we click on a building
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        //we need to deal with placing a building before we deal with selecting or releasing
        if(current != null)//if we have selected a building
        {
            if(Physics.Raycast(mouseRay, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Terrain")))
                //if we are on the current terrain, continue
            {
                //we will want to set our current building's position to be where the ray hits our terrain
                //we also want the grid to show up

                //let's make our point to snap to the grid (luckily our grid is of value 1)
                Vector3 pos = hitInfo.point;
                pos.x = Mathf.Round(pos.x);
                pos.y = current.transform.position.y;
                pos.z = Mathf.Round(pos.z);
                current.transform.position = pos;
            }
            gridAlpha = Mathf.MoveTowards(gridAlpha, 1, Time.deltaTime * gridFadeSpeed); //make sure to set grid fade speed in the inspector
        }
        else
        {
            gridAlpha = Mathf.MoveTowards(gridAlpha, 0, Time.deltaTime * gridFadeSpeed); //if we are not selecting anything, move towards 0 for the alpha 
        }
        grid.material.SetColor("_LineColor", new Color(1, 1, 1, gridAlpha)); //we set the material colour every time we do an update

        if (Input.GetMouseButtonDown(0))
        {
            //first check if we have a current building that we are working with
            if(current == null)
            {
                //let's start by creating a ray object from the ray position in the world
                if (Physics.Raycast(mouseRay, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Building")))
                {
                    current = hitInfo.collider.GetComponent<Building>();
                   // Debug.Log("We hit a building");
                }
            }
            else
            {
                //this will be our code for placing a building
                if (!current.IsOverlapping)
                {
                    current = null; //only release the building if it is not overlapping another building
                    buildingAdded.Invoke(); //this broadcasts that the event has happened
                }
            }

        }
    }
    public void SetCurrent(Building building)
    {
        if(current != null)
        {
            Destroy(current.gameObject);
        }
        current = Instantiate(building);
        current.name = building.name;
    }
}
