using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    //Our pool manager is going to be used for spawning pool objects. This means, however, that we are not creating objects through pure instantiation; we are instead doing it based on what we have available
    //As well, our pool manager is going to create a bunch of pools based on our folders, and, in particular, our PoolObjects folder in the resource directory
    Dictionary<string, Stack<PoolObject>> stackDictionary = new Dictionary<string, Stack<PoolObject>>();
    // Start is called before the first frame update
    void Start()
    {
        PoolManager.Instance.Load();
    }
    private void Load()
    {
        //this is where we create our Dictionary of pool objects (stacks) that are named. The way we will do this is by finding all of our PoolObjects and then, for each pool object, we will create its own stack, and then we will name those stacks
        PoolObject[] poolObjects = Resources.LoadAll<PoolObject>("PoolObjects");
        foreach(PoolObject poolObject in poolObjects)
        {
            Stack<PoolObject> objStack = new Stack<PoolObject>();
            objStack.Push(poolObject);//push puts something into the Stack
            stackDictionary.Add(poolObject.name, objStack); //this adds the particular pool object's stack to the dictionary (which means it can be referred to by name)
        }
    }
    public PoolObject Spawn(string name) //this returns a pool object based on the name we ask for
    {
        Stack<PoolObject> objStack = stackDictionary[name];
        //we have two situations: our objStack has 1 item, so we need to make a new object
        //our objStack has more than one item, so we give back one of the objects in the pool
        if(objStack.Count == 1)
        {
            PoolObject poolObject = objStack.Peek();//look at the top item
            PoolObject objectClone = Instantiate(poolObject);
            objectClone.name = poolObject.name;//make sure all objects have the same name
            return objectClone;
        }
        PoolObject oldPoolObject = objStack.Pop();
        oldPoolObject.gameObject.SetActive(true);
        return oldPoolObject;
    }
    public void DeSpawn(PoolObject poolObject)
    {
        Stack<PoolObject> objStack = stackDictionary[poolObject.name];
        //make sure the object is inactive so it's not showing up in the scene
        poolObject.gameObject.SetActive(false);
        objStack.Push(poolObject); //now we have pushed in the old object and it can be used later
    }

}
