using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : Singleton<WorldManager>
{
    public WorldData data;
    // Start is called before the first frame update
    void Start()
    {
        Load();
    }
    private void Load()
    {
        foreach(BuildingData bData in data.buildings)
        {
            Building buildingPrefab = Resources.Load<Building>("Buildings/" + bData.name);
            Building buildingClone = Instantiate(buildingPrefab); //clone the object
            buildingClone.name = bData.name; //we will need this for saving it later
            buildingClone.transform.position = bData.position;
        }
    }
    public void SaveData()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
