using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : Singleton<SpellManager>
{
    [SerializeField]
    private GameObject spell;
    [SerializeField]
    private float cooldownDuration = 2f;

    void Update()
    {
        cooldownDuration -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Alpha1) && cooldownDuration <=0)
        {
            cooldownDuration = 2f;
            SpawnSpellAtMousePos(); 
        }
    }

    private void SpawnSpellAtMousePos()
    {
        if (spell == null)
            Debug.LogError(" need Spell to be added to SpellManager");
        else
        Instantiate(spell, MouseHitPos(), transform.rotation);
    }

    private Vector3 MouseHitPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}