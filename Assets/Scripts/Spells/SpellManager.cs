using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    [System.Serializable]
    public class Spell
    {
        public GameObject prefab; // Prefab for the spell
        public KeyCode key;       // Key to trigger the spell
    }

    public List<Spell> spells; // List of spells
    public LayerMask unitLayer; // LayerMask to target units

    void Update()
    {
        foreach (var spell in spells)
        {
            if (Input.GetKeyDown(spell.key))
            {
                CastSpellAtMousePosition(spell);
            }
        }
    }

    void CastSpellAtMousePosition(Spell spell)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, unitLayer))
        {
            Instantiate(spell.prefab, hit.point, Quaternion.identity);
        }
    }
}
