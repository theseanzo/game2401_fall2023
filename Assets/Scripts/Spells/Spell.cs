using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{

    public virtual void Cast()
    {
        Debug.Log("spell cast");
    }

    public virtual void Effect()
    {

    }
}
