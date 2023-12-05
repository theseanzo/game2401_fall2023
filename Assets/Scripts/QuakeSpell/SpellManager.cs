using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public GameObject spellPrefab;
    public float effectRadius = 5f;
    public LayerMask buildingLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CastSpellAtMousePosition();
        }
    }

    void CastSpellAtMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Instantiate(spellPrefab, hit.point, Quaternion.identity);
        }
    }
}
