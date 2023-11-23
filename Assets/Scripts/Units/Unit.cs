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
    public float moveSpeed = 1f;
    [SerializeField]
    float rotationSpeed = 1f;
    [SerializeField]
    float attackRange = 1f; //basic unit needs to be up close
    [SerializeField]
    public float attackInterval = 2f; //how long before they can attack again (avoid constant attacking)
    [SerializeField]
    public int attackPower = 10;

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

    protected override void Start()
    {
        base.Start();
        //we need to set up our last position, our seeker, our animator and then set the current state
        lastPos = transform.position;  //where we were last seen
        seeker = GetComponent<Seeker>(); //find the seeker and set it up
        anim = GetComponentInChildren<Animator>(); //find the animator in the children (it's in the child components because our main unit does not have an animator, since the animator is on the model
        SetState(OnIdle()); //this starts our coroutines
    }

    private void FixedUpdate() //this is our infamous FixedUpdate function (it is called on the fixed interval rather than our Update)
    {
        Vector3 movement = transform.position - lastPos;
        anim.SetFloat("Speed", movement.magnitude); //we now set out speed to be based on how far we moved.
        lastPos = transform.position;
    }
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
            LookForBuilding(); //we no longer need to worry about exiting this coroutine, because, if a building is found, it will change our state for us
        }
    }

    IEnumerator OnMoveToTarget(Building target)
    {
        //We use a seeker to calculate the path to our target. When the path has been calculated, it calls a method. What this means is we need to provide a delegate function for our seeker's StartPath method
        seeker.StartPath(transform.position, target.transform.position, p => currentPath = p); //this uses a lambda function to create an anonymous (or inline function) that fulfills the fact that our seeker.StartPath requires a function to be called when its StartPath is finished. What we want is to store the currentPath from our seeker when we have found it
        //the p=>currentPath = p is essentially the same as saying: someFunction(Path p){ currentPath = p;}
        //if you are required to have a function passed in as a parameter, and you know that function is small and will only exist once, feel free to use a lambda function
        while (true)
        {
            yield return new WaitForFixedUpdate();

            if(currentPath != null) //make sure we have actually found a path
            {
                //we know that, on our path, we are going to move between different points on our path and we will move according to our speed. With that, we need to simply move until we get to the end of our path

                //let's make sure our current target has not been destroyed
                if(target == null)
                {
                    SetState(OnIdle());
                }
                Vector3 nextPoint = currentPath.vectorPath[currentIndex];//our vectorPath in the path is the list of points along our path
                transform.position = Vector3.MoveTowards(transform.position, nextPoint, moveSpeed * Time.fixedDeltaTime); //we are moving according to a fixed delta time (our fixed framerate, and we use the fixed frame rate because we WaitForFixedUpdate 
                LookTowards(nextPoint);
                if(transform.position == nextPoint)
                {
                    currentIndex++; ///we have got the next point so we need to look for another point
                }

                if(currentIndex >= currentPath.vectorPath.Count)
                {
                    currentIndex = 0;
                    currentPath = null;
                }
                //Instead of going towards the center of the building, we are going to go the closest point of the building to use; this ensures that our units will attack the sides of the building rather than the center
                Vector3 targetPos = target.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                if(Vector3.Distance(transform.position, targetPos) <= attackRange) //if we get within range of our building, attack
                {
                    SetState(OnAttack(target));
                }
            }
        }
    }
    private void LookTowards(Vector3 position)
    {
        //we are going to nneed to calculate where we are rotating towards according to our current position
        Vector3 targetDirection = position - transform.position; //the vector from our position to the position of our target
        //we only want to rotate if that value is something (i.e. if it's 0 don't rotate)
        if(targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime); //we are essentially blending our rotation time by our current rotation value
        }
    }

    IEnumerator OnAttack(Building target)
    {
        attackTarget = target;
        while (true)
        {
            //we want to attack our target after our interval 

            lastAttackTime += Time.deltaTime;
            //what do do if our building has been destroyed? We idle again
            if(attackTarget == null)
            {
                SetState(OnIdle());
            }
            LookTowards(attackTarget.GetComponent<Collider>().bounds.center);
            if (lastAttackTime >= attackInterval)
            {
                lastAttackTime = 0;
                Attack();
                //Attack
            }
            yield return new WaitForFixedUpdate();
        }
    }
    protected virtual void Attack()
    {
        anim.SetTrigger("Attack"); //"Attack" is the name for the trigger on our animation
    }
    private void LookForBuilding() //this is our function for finding a building when in our idle state and then we are going to transition to our MoveToTarget state if we find a building
    {
        //to decide on the building we are going to attack, we are going to go with the simplest case which is just to choose the building that is closest
        Building[] allBuildings = FindObjectsOfType<Building>(); //question: what might be a more efficient way to do this? We could have our building manager keep track of our buildings
        //we want to find the building that is closest to our character, and set that to be our current target
        float shortestDistance = Mathf.Infinity;
        Building closestBuilding = null; //currently we don't have a closest building so it is null
        foreach(Building building in allBuildings)
        {
            float distance = Vector3.Distance(transform.position, building.transform.position); //get the distance from our unit to the building
            if(distance < shortestDistance)
            {
                shortestDistance = distance;
                closestBuilding = building;
            }
        }//by the end of the loop we will either have the closest building or we will have no building at all (because all of the buildings are destroyed)
        if (closestBuilding != null)
            SetState(OnMoveToTarget(closestBuilding)); //here we are going to change our state to OnMoveToTarget using the closest building if and only if we found a building
    }
    
    // Start is called before the first frame update

    public virtual void OnAttackActionEvent()
    {
        //this will be called by our animation, and will calculate how we do damage
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
