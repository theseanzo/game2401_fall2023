using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScreen : MonoBehaviour
{
    [SerializeField]
    DynamicList unitButtons; //let's specify which unit we are going to use
    DynamicList spellButtons;
    // Start is called before the first frame update
    private void Awake()
    {
        gameObject.SetActive(GameManager.Instance.CurrentState == GameState.Attacking);
    }
    void Start()
    {
        GameObject[] units = Resources.LoadAll<GameObject>("Units"); //grab all of the game objects from the units folder
        unitButtons.CreateButtons(units);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnBuildButton()
    {
        GameManager.Instance.CurrentState = GameState.Building;
    }
    public void OnAttackButton(ListButton button)
    {
        AttackManager.Instance.SetCurrent(button.linkedObject.GetComponent<Unit>());
        //BuildingManager.Instance.SetCurrent(button.linkedObject.GetComponent<Building>());
    }
}
