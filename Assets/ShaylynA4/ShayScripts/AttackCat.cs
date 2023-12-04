using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCat : Unit
{
    // These were also supposed to attack, but I'm not sure I can do that since they aren't animated (and aren't humanoid.) Their purpose is now to swarm buildings and be annoying.

    public float Lifetime = 10f;

    private void Awake()
    {
        Destroy(gameObject, Lifetime); // Destroys the cat after 10 seconds
    }

}
