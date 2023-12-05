using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public GameObject spellParticle;
    //public float effectRadius = 5f; // radius for the spell effect
    //public LayerMask buildingLayer; // layer mask to identify buildings
    private void Awake()
    {
        StartCoroutine(NoMoreParticle());
    }
    void Update()
    {
        // the 1 key is pressed down
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CastSpell(); // cast a spell at the mouse position
        }
    }

    void CastSpell()
    {
        // create a ray from the camera to the mouse position
        Ray castRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        // perform a raycast to check if it hits anything
        if (Physics.Raycast(castRay, out hitInfo))
        {
            // instantiate the spell prefab at the hit point
            Instantiate(spellParticle, hitInfo.point, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    IEnumerator NoMoreParticle()
    {
        yield return new WaitForSeconds(2);
        Object.Destroy(this.gameObject);
    }
}
