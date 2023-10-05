using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//we need all units to have a Seeker component. How do we make sure that all units will have this?
[RequireComponent(typeof(Seeker))]
public class Unit : BaseObject
{
    //for our unit we're going to need a move speed; we're going to need a rotation speed, their attack range, their attack interval, their power
    [SerializeField]
    float moveSpeed = 1f;
    [SerializeField]
    float rotationSpeed = 1f;
    [SerializeField]
    float attackRange = 1f; //basic unit needs to be up close
    [SerializeField]
    float attackInterval = 2f; //how long before they can attack again (avoid constant attacking)
    [SerializeField]
    float attackPower = 10f;

    //when we attack, we will need to keep track of when we have attacked last to see how long has elapsed
    private float lastAttackTime = 0f;//this will be used when we are attacking to make sure we wait for the interval to end

    //We will need to keep track of a couple components
    protected Seeker seeker;//this is protected so that our children can keep track of the seeker
    protected Animator anim; //we want to keep track of the animator as well in our children

    //some run time variables we will need: we need to keep track of the current path (there is a Path object included with the A* library). We need to keep track of the current index in our path we are at. We will also need to keep track of the current coroutine we are setting for our character (i.e. the state of our character), and the last position of our character. Oh, and we also need to know the current target we are aiming for
    private Path currentPath;
    private int currentIndex = 0;
    private Coroutine currentState;
    private Vector3 lastPos;
    protected Building attackTarget; //we could also set this to be a BaseObject in the future if we want to include Unit combat
    
    private void SetState(IEnumerator newState) //when we change states, we stop our previous coroutine and then initialize a new one. Technically this can be done with "StopAllCoroutines()" because we only have one coroutine running, but in the case that we had more, we will only a stop a specific coroutine.
    {
        if(currentState != null)
        {
            StopCoroutine(currentState);
        }
        currentState = StartCoroutine(newState);
    }
    //Remember that a Coroutine allows us to pause our execution and return to executing at that spot at a certain period in time
    IEnumerator OnIdle() //recall that, for a coroutine, we require it to have the return type of IEnumerator
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();//this will pause and wait for a FixedUpdate to finish
            //Look for a new target 
        }
    }

    IEnumerator OnMoveToTarget(Building target)
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator OnAttack(Building target)
    {
        attackTarget = target;
        while (true)
        {
            yield return new WaitForFixedUpdate();
        }
    }

    
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
