using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ParalyzingFogSpawn : MonoBehaviour
{
    public float spawnDelay = 5f;
    public GameObject prefabToSpawn;
    public Button spawnButton;

    private bool canSpawn = false;

    void Start()
    {
        if (prefabToSpawn == null)
        {
            Debug.LogError("Prefab not assigned to the spawn button.");
            enabled = false;
        }
        else
        {
            spawnButton.onClick.AddListener(OnButtonClicked);
            spawnButton.interactable = true;
            canSpawn = false;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canSpawn && !EventSystem.current.IsPointerOverGameObject())
            {
                OnMapClicked();
            }
        }
    }

    void OnMapClicked()
    {
        // Check if the mouse is not over a UI element
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            // Disable the button during the spawnDelay
            spawnButton.interactable = false;
            canSpawn = false;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject spawnedObject = Instantiate(prefabToSpawn, hit.point, Quaternion.identity);

                StartCoroutine(SpawnFog(spawnedObject));
            }
        }
    }

    IEnumerator SpawnFog(GameObject spawnedObject)
    {
        yield return new WaitForSeconds(spawnDelay);

        Destroy(spawnedObject);

        // Enable the button after the spawnDelay
        spawnButton.interactable = true;
        enabled = false;
    }

    public void OnButtonClickedHelper()
    {
        ActivateScript();
    }

    public void OnButtonClicked()
    {
        if (canSpawn)
        {
            OnMapClicked();
        }
    }

    public void ActivateScript()
    {
        enabled = true;
        canSpawn = true;
    }
}
