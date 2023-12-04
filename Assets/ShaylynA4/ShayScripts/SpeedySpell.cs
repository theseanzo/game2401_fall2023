using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedySpell : MonoBehaviour
{
    public float Lifetime = 4f;

    private void Update()
    {
        Destroy(this.gameObject, Lifetime); // Destroys the spell after a set period of time
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Unit>())
        {

        }
    }
}
