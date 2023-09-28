using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicList : MonoBehaviour
{
    [SerializeField]
    ListButton originalButton; //we are going to clone this button over and over
    // Start is called before the first frame update
    public void CreateButtons(GameObject[] objs)
    {
        foreach(GameObject obj in objs)
        {
            ListButton buttonClone = Instantiate(originalButton, originalButton.transform.parent);
            buttonClone.gameObject.SetActive(true); //we will need to make sure to enable our clones
            buttonClone.Init(obj); //initialize the button
        }
    }

    void Start()
    {
        originalButton.gameObject.SetActive(false); //make sure to hide our original button
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
