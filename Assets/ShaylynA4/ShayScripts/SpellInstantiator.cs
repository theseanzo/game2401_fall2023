using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellInstantiator : MonoBehaviour
{
    public GameObject Spell;

    private void Awake()
    {
       // DontDestroyOnLoad(this); // Makes sure the object containing this script isn't destroyed on load
        //Spell = GameObject.Find("ShaySpeedySpell"); // Finds the spell prefab
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) // if the "1" key is pressed...
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); // Casts a ray into the scene from the camera
            RaycastHit hit;

            if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain"))) // If the ray hits the terrain...
            {
                Vector3 pos = hit.point;
                Instantiate(Spell, pos, Quaternion.identity); // Instantiates the spell where the terrain is hit
            }
        }
    }
}
