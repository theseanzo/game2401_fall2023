using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public GameObject spellPrefab; // reference to the spell prefab to be instantiated
    public float effectRadius = 5f; // radius for the spell effect
    public LayerMask buildingLayer; // layer mask to identify buildings

    void Update()
    {
        // the 1 key is pressed down
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CastSpellAtMousePosition(); // cast a spell at the mouse position
        }
    }

    void CastSpellAtMousePosition()
    {
        // create a ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // perform a raycast to check if it hits anything
        if (Physics.Raycast(ray, out hit))
        {
            // instantiate the spell prefab at the hit point
            Instantiate(spellPrefab, hit.point, Quaternion.identity);
        }
    }
}
