using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    // Start is called before the first frame update
    //Our basic object is going to have some health and it's going to be able to be destroyed
    //We will need to be something other objects can look at, and if this is the case, how should we define our health? Assuming it is an integer
    public int Health
    {
        get; protected set; //a protected set means that only our object and its children can change its health
    }
    public virtual void OnHit(int damage) //for our OnHit, we may want to change this in our children and not use this function. What do we do need to do to allow our children to potentially override, or even call, this function? The function needs to be...virtual. A virtual function is one that can be overriden by children
    {
        Health -= damage;
        if(Health <= 0)
        {
            //we need to do something for dying
        }
    }
    public virtual void OnDie() //we will call the OnDie function when we want our character to die
    {
        Destroy(this.gameObject); //we could also just say gameObject (I wanted to make explicit that every MonoBehaviour has a gameObject associated with it
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
