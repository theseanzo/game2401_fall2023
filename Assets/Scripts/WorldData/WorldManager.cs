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
    public void DelayedSave()
    {
        Invoke("SaveData", 0.1f);
    }
    public void SaveData()
    {
        //recreate the list of buildings
        data.buildings = new List<BuildingData>();
        Building[] allBuildings = FindObjectsOfType<Building>();
        foreach(Building building in allBuildings)
        {
            BuildingData bData = new BuildingData();
            bData.name = building.name;
            bData.position = building.transform.position;
            data.buildings.Add(bData);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
