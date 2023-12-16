using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZoomSpell : MonoBehaviour
{
    public GameObject current;
    [SerializeField]
    private int spellQuantity = 1;
    [SerializeField]
    private float spellEffect = 2;
    [SerializeField]
    private float spellRange = 5;
    [SerializeField]
    private float spellTime = 5;
    [SerializeField]
    private float spellOvertime = 2;

    Ray mouseRay;
    RaycastHit hitInfo;

    [SerializeField]
    UnityEvent onTriggerEnter;
    [SerializeField]
    UnityEvent onTriggerExit;

    // Update is called once per frame
    void Update()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("You Pressed 1");
            // outline of the spell to see where it'll place before you place it
            if (spellQuantity >= 1)
            {
                spellQuantity = spellQuantity - 1;
                if(Physics.Raycast(mouseRay, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Terrain")))
                {
                    Vector3 pos = hitInfo.point;
                    pos.x = Mathf.Round(pos.x);
                    pos.y = current.transform.position.y;
                    pos.z = Mathf.Round(pos.z);
                    current.transform.position = pos;
                }
            }
            else
            {
                Debug.Log("You have no spells left");
            }

            if (Input.GetMouseButtonDown(0))
            {
                current = null;
                SpellCast();
            }
        }
    }

    
    
        
    

    void SpellCast()
    {
        //once spell is placed it'll stay for a duration and cast on anything that enters it until it is up giving.
        float spellDuration = Time.time;
        if (spellDuration >= spellTime)
        {
            //OnTriggerEnter();
            Instantiate(current);
        }
        else
        {
            Destroy(current);
        }
    }

   /* void OnTriggerEnter(Collider other)
    {
        //when a unit enters the radius it'll gain a speed boost
        //problem is I do not know how to single out the unit triggering the event to give them the speed boost.
        onTriggerEnter.Invoke();
    }

    void OnTriggerExit(Collider other)
    {
        //when it leaves it'll retain that speed boost for a short duration
        onTriggerExit.Invoke();
    }*/
}
