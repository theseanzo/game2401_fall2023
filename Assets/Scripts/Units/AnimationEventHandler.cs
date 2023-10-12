using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    //it will need a reference to the unit that triggers the event
    //and then we will need to have some sort of function that captures our "AttackAnimationEvent"; which means we need a function "AttackAnimationEvent"
    private Unit unit;
    // Start is called before the first frame update
    void Awake()
    {
        unit = GetComponentInParent<Unit>();//we know that our parent has the type Unit, so find that Unit
    }
    public void AttackAnimationEvent()
    {
        //we will need to do something here
        unit.OnAttackActionEvent();
    }

}
