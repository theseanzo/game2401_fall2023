using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldData", menuName = "ScriptableObjects/CreateWorldData")]
public class WorldData : ScriptableObject
{
    public List<BuildingData> buildings;
}
<<<<<<< Updated upstream
[System.Serializable] //allow it to be seen in the inspector
=======
[System.Serializable]
>>>>>>> Stashed changes
public struct BuildingData //
{
    public string name;
    public Vector3 position;
}
