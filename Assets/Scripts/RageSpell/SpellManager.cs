using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public GameObject rageSpellPrefab; // Prefab for the rage spell
    public float effectRadius = 5f;
    public LayerMask unitLayer; // LayerMask to target units

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CastRageSpellAtMousePosition();
        }
    }

    void CastRageSpellAtMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayer))
        {
            GameObject spellInstance = Instantiate(rageSpellPrefab, hit.point, Quaternion.identity);
        }
    }
}
