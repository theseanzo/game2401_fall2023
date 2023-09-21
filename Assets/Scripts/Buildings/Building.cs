using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class Building : MonoBehaviour
{
    public Vector2Int size; //vector2 int means that we have set up our buildings to be a particular size for grid locations
    private float colliderHeight = 2f; //We are going to use a fixed height for our colliders
    // Start is called before the first frame update
    private void Awake() //recall that awake is called when the script is loaded
    {
        this.gameObject.layer = LayerMask.NameToLayer("Building"); //after: let's make sure that we actually create a Building layer, but this is needed in order for our buildings to be on a building layer
    }
    private void OnValidate()
    {
        //we recompute our box collider
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.size = new Vector3(size.x, colliderHeight, size.y);
        collider.center = new Vector3(0, colliderHeight * 0.5f, 0); 
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
