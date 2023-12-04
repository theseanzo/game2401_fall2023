using System.Collections;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField]
    private GameObject spell;
    private Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        spell.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //pressing "Alpha1" key to activate spell and place them at mouse position
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.WorldToScreenPoint(Input.mousePosition);
            spell.SetActive(true);
            spell.transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
        }
        if (Input.GetMouseButtonUp(0))
        {
            spell.SetActive(false);
        }
    }
}
