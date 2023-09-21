using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldData", menuName = "ScriptableObjects/CreateWorldData")]
public class WorldData : ScriptableObject
{
    public List<BuildingData> buildings;
}
[System.Serializable] //allow it to be seen in the inspector
public struct BuildingData //
{
    public string name;
    public Vector3 position;
}
