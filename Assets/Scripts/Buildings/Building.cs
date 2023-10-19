using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class Building : BaseObject
{
    public UnityEvent buildingDestroyed;
    public Vector2Int size; //vector2 int means that we have set up our buildings to be a particular size for grid locations
    private float colliderHeight = 2f; //We are going to use a fixed height for our colliders
    // Start is called before the first frame update

    private int nOverLappingBuildings = 0;
    private bool _isOverlapping = false; //the _ indicates it is a field for the property
    public bool IsOverlapping { 
        get 
        {
            return _isOverlapping;
        } 
        set
        {
            _isOverlapping = value; //recall that a value is what something is set to (i.e. IsOverlapping = something, and the something is the value
            if (_isOverlapping)
                rd.material.color = Color.red;
            else
                rd.material.color = Color.white;
        }
    }

    private MeshRenderer rd;
    private void Awake() //recall that awake is called when the script is loaded
    {
        this.gameObject.layer = LayerMask.NameToLayer("Building"); //after: let's make sure that we actually create a Building layer, but this is needed in order for our buildings to be on a building layer
        rd = GetComponentInChildren<MeshRenderer>();
        buildingDestroyed.AddListener(WorldManager.Instance.DelayedSave);//our world manager will now listen for a building being destroyed
        
    }
    private void OnValidate()
    {
        //we recompute our box collider
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.size = new Vector3(size.x, colliderHeight, size.y);
        collider.center = new Vector3(0, colliderHeight * 0.5f, 0);
        collider.isTrigger = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        
    }
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Building>())
        {
            nOverLappingBuildings++;
            IsOverlapping = true;

        }  
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Building>())
        {
            nOverLappingBuildings--;
            if(nOverLappingBuildings == 0)
            {
                IsOverlapping = false;
            }

        }
    }
    public override void OnDie()
    {
        base.OnDie(); //call our parent's death first. This guarantees that the building's game object is destroyed
        Destroy(this.gameObject);
        buildingDestroyed.Invoke();
        Debug.Log("We got here");
    }
}
