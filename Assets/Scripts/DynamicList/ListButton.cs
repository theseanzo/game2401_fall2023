using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ListButton : MonoBehaviour
{
    [HideInInspector]
    public GameObject linkedObject;
    // Start is called before the first frame update
    public void Init(GameObject obj)
    {
        linkedObject = obj;
        //we're going to need a linked object's name to be attached to the button (since it's dynamic)
        GetComponentInChildren<TMP_Text>().text = linkedObject.name;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
