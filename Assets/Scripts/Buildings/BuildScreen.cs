using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildScreen : MonoBehaviour
{
    [SerializeField]
    DynamicList buildingButtons; //let's specify which dynamic we are going to use
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] buildings = Resources.LoadAll<GameObject>("Buildings"); //grab all of the game objects from the buildings folder
        buildingButtons.CreateButtons(buildings);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnBuildButton(ListButton button)
    {
        BuildingManager.Instance.SetCurrent(button.linkedObject.GetComponent<Building>());
    }
}
