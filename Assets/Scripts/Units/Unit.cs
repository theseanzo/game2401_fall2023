using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// We ensure that all units have a Seeker component.
// How do we make sure that all units will have this?
[RequireComponent(typeof(Seeker))]
public class Unit : BaseObject
{
    // Properties for the unit
    public float moveSpeed = 1f;
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] float attackRange = 1f; // Basic unit needs to be up close
    [SerializeField] float attackInterval = 2f; // How long before they can attack again (avoid constant attacking)
    [SerializeField] protected int attackPower = 10;

    private float originalMoveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    // Variables to keep track of attacks
    private float lastAttackTime = 0f; // This will be used when we are attacking to make sure we wait for the interval to end

    // Components
    protected Seeker seeker; // This is protected so that our children can keep track of the seeker
    protected Animator anim; // We want to keep track of the animator as well in our children

    // Runtime variables we will need
    // We need to keep track of the current path (there is a Path object included with the A* library).
    // We need to keep track of the current index in our path we are at.
    // We will also need to keep track of the current coroutine we are setting for our character (i.e. the state of our character),
    // and the last position of our character.
    // Oh, and we also need to know the current target we are aiming for.
    private Path currentPath;
    private int currentIndex = 0;
    private Coroutine currentState;
    private Vector3 lastPos;
    protected Building attackTarget; // We could also set this to be a BaseObject in the future if we want to include Unit combat

    protected override void Start()
    {
        base.Start();
        // We need to set up our last position, our seeker, our animator and then set the current state
        lastPos = transform.position;  // Where we were last seen
        seeker = GetComponent<Seeker>(); // Find the seeker and set it up
        anim = GetComponentInChildren<Animator>(); // Find the animator in the children (it's in the child components because our main unit does not have an animator, since the animator is on the model
        originalMoveSpeed = moveSpeed;
        SetState(OnIdle()); // This starts our coroutines
    }

    private void FixedUpdate() // This is our infamous FixedUpdate function (it is called on the fixed interval rather than our Update)
    {
        Vector3 movement = transform.position - lastPos;
        anim.SetFloat("Speed", movement.magnitude); // We now set out speed to be based on how far we moved.
        lastPos = transform.position;
    }

    private void SetState(IEnumerator newState) // When we change states, we stop our previous coroutine and then initialize a new one. Technically this can be done with "StopAllCoroutines()" because we only have one coroutine running, but in the case that we had more, we will only stop a specific coroutine.
    {
        if (currentState != null)
        {
            StopCoroutine(currentState);
        }
        currentState = StartCoroutine(newState);
    }

    // Remember that a Coroutine allows us to pause our execution and return to executing at that spot at a certain period in time
    IEnumerator OnIdle() // Recall that, for a coroutine, we require it to have the return type of IEnumerator
    {
        while (true)
        {
            yield return new WaitForFixedUpdate(); // This will pause and wait for a FixedUpdate to finish
            // Look for a new target 
            LookForBuilding(); // We no longer need to worry about exiting this coroutine because, if a building is found, it will change our state for us
        }
    }

    IEnumerator OnMoveToTarget(Building target)
    {
        // We use a seeker to calculate the path to our target.
        // When the path has been calculated, it calls a method.
        // What this means is we need to provide a delegate function for our seeker's StartPath method
        seeker.StartPath(transform.position, target.transform.position, p => currentPath = p); // This uses a lambda function to create an anonymous (or inline function) that fulfills the fact that our seeker.StartPath requires a function to be called when its StartPath is finished.
        // What we want is to store the currentPath from our seeker when we have found it
        // The p=>currentPath = p is essentially the same as saying: someFunction(Path p){ currentPath = p;}
        // If you are required to have a function passed in as a parameter, and you know that function is small and will only exist once, feel free to use a lambda function
        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (currentPath != null) // Make sure we have actually found a path
            {
                // We know that, on our path, we are going to move between different points on our path and we will move according to our speed.
                // With that, we need to simply move until we get to the end of our path

                // Let's make sure our current target has not been destroyed
                if (target == null)
                {
                    SetState(OnIdle());
                }
                Vector3 nextPoint = currentPath.vectorPath[currentIndex]; // Our vectorPath in the path is the list of points along our path
                transform.position = Vector3.MoveTowards(transform.position, nextPoint, moveSpeed * Time.fixedDeltaTime); // We are moving according to a fixed delta time (our fixed framerate, and we use the fixed frame rate because we WaitForFixedUpdate 
                LookTowards(nextPoint);
                if (transform.position == nextPoint)
                {
                    currentIndex++; /// We have got the next point so we need to look for another point
                }

                if (currentIndex >= currentPath.vectorPath.Count)
                {
                    currentIndex = 0;
                    currentPath = null;
                }
                // Instead of going towards the center of the building, we are going to go the closest point of the building to use;
                // This ensures that our units will attack the sides of the building rather than the center
                Vector3 targetPos = target.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                if (Vector3.Distance(transform.position, targetPos) <= attackRange) // If we get within range of our building, attack
                {
                    SetState(OnAttack(target));
                }
            }
        }
    }

    private void LookTowards(Vector3 position)
    {
        // We are going to need to calculate where we are rotating towards according to our current position
        Vector3 targetDirection = position - transform.position; // The vector from our position to the position of our target
        // We only want to rotate if that value is something (i.e., if it's 0 don't rotate)
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime); // We are essentially blending our rotation time by our current rotation value
        }
    }

    IEnumerator OnAttack(Building target)
    {
        attackTarget = target;
        while (true)
        {
            // We want to attack our target after our interval 

            lastAttackTime += Time.deltaTime;
            // What to do if our building has been destroyed? We idle again
            if (attackTarget == null)
            {
                SetState(OnIdle());
            }
            LookTowards(attackTarget.GetComponent<Collider>().bounds.center);
            if (lastAttackTime >= attackInterval)
            {
                lastAttackTime = 0;
                Attack();
                // Attack
            }
            yield return new WaitForFixedUpdate();
        }
    }

    protected virtual void Attack()
    {
        anim.SetTrigger("Attack"); // "Attack" is the name for the trigger on our animation
    }

    private void LookForBuilding() // This is our function for finding a building when in our idle state.
                                   // Then we are going to transition to our MoveToTarget state if we find a building
    {
        // To decide on the building we are going to attack, we are going to go with the simplest case,
        // which is just to choose the building that is closest
        Building[] allBuildings = FindObjectsOfType<Building>(); // Question: what might be a more efficient way to do this?
                                                                 // We could have our building manager keep track of our buildings
                                                                 // We want to find the building that is closest to our character, and set that to be our current target
        float shortestDistance = Mathf.Infinity;
        Building closestBuilding = null; // Currently we don't have a closest building, so it is null
        foreach (Building building in allBuildings)
        {
            float distance = Vector3.Distance(transform.position, building.transform.position); // Get the distance from our unit to the building
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestBuilding = building;
            }
        } // By the end of the loop we will either have the closest building or we will have no building at all
          // (because all of the buildings are destroyed)
        if (closestBuilding != null)
            SetState(OnMoveToTarget(closestBuilding)); // Here we are going to change our state to OnMoveToTarget using the closest building if and only if we found a building
    }

    // Start is called before the first frame update

    public virtual void OnAttackActionEvent()
    {
        // This will be called by our animation, and will calculate how we do damage
    }

    // Update is called once per frame
    void Update()
    {
        // ...
    }

    public void RestoreOriginalSpeed()
    {
        moveSpeed = originalMoveSpeed;
    }
}
