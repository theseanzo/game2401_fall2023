using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    [SerializeField] private Spell[] spell = new Spell[10];
    private Spell _current;

    private KeyCode[] keyCodes = {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
        KeyCode.Alpha0
    };

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            int keyNum = FindKey();
            if (keyNum >= 0)
            {
                _current = spell[keyNum];
            }
        }
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        //we need to deal with placing a building before we deal with selecting or releasing
        if (_current != null)//if we have selected a building
        {
            if (Physics.Raycast(mouseRay, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Terrain")))
            //if we are on the current terrain, continue
            {
                Vector3 pos = hitInfo.point;
                pos.x = Mathf.Round(pos.x);
                pos.y = _current.transform.position.y;
                pos.z = Mathf.Round(pos.z);
                _current.transform.position = pos;
            }

        }
    }
        private int FindKey()
        {
            int index = -1;
            foreach (int i in keyCodes)
            {
                if (Input.GetKeyDown(keyCodes[i]))
                {
                    index = i;
                    break;
                }

            }
            return index;
        }
    
}
