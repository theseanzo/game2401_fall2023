using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasteSpell : MonoBehaviour
{
    [SerializeField]
    private GameObject hasteBuffEffect; //the particle effect that is spawned in a unit when sped up by the haste spell

    [SerializeField]
    private float hasteSpeed;//the speed that the haste spell speeds up units by

    [SerializeField]
    private float hasteDuration;

    [SerializeField]
    private float hasteRadius;

    List<Unit> fastUnits = new();//list of units that have been sped up by the haste spell
    List<GameObject> hasteParticles = new ();//list of particles that have been spawned by the haste spell

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, hasteRadius); //creates an array of colliders that are within the haste radius

        foreach (Collider collider in colliders) //IF collided object is a unit and is not already in the list of sped up units, add it to the list and speed it up
        {
            Unit unit = collider.gameObject.GetComponent<Unit>();
            if (unit && !fastUnits.Contains(unit))
            {
                fastUnits.Add(unit);
                unit.moveSpeed += hasteSpeed;
                unit.attackInterval -= hasteSpeed;
                unit.attackInterval = Mathf.Min(1,unit.attackInterval);
                GameObject effect = Instantiate(hasteBuffEffect, unit.transform.position, Quaternion.identity);
                effect.transform.parent = unit.transform;
                hasteParticles.Add(effect);
            }
        }

        hasteDuration -= Time.deltaTime;

        if (hasteDuration <= 0) //when the haste spell has run out, remove the haste buff from all units and destroy the haste spell
        {
            for (int i = fastUnits.Count - 1; i >= 0; i--)
            {
                Unit unit = fastUnits[i];
                unit.attackInterval += hasteSpeed;
                unit.moveSpeed -= hasteSpeed;
                fastUnits.RemoveAt(i);
            }

            for (int i = hasteParticles.Count - 1; i >= 0; i--)
            {
                GameObject particle = hasteParticles[i];
                hasteParticles.RemoveAt(i);
                Destroy(particle);
            }

            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos() //draws a sphere around the object in the editor
    {
        Gizmos.DrawWireSphere(transform.position, hasteRadius); //remove after testing 
    }
}
