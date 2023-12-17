using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : Singleton<AttackManager>
{
    public Unit currentPrefab;//this will be the unit we are currently placing
    private bool canSpawn = true;

    // Start is called before the first frame update
    private void Awake()
    {
        gameObject.SetActive(GameManager.Instance.CurrentState == GameState.Attacking);
    }
    void Start()
    {
        
    }
    public void SetCurrent(Unit unitPrefab)
    {
        currentPrefab = unitPrefab;  //this will set up whatever unit we are going to be placing
        canSpawn = true;
    }
    // Update is called once per frame
    void Update() //here we are going to do all of the magic we did before except not base ont the grid
    {
        if (canSpawn && currentPrefab != null && Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(mouseRay, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Terrain")))
            {
                Vector3 pos = hitInfo.point;
                Instantiate(currentPrefab, pos, Quaternion.identity);
                canSpawn = false;  // Desativa a capacidade de spawn após a unidade ser spawnada
            }
        }
    }
}
