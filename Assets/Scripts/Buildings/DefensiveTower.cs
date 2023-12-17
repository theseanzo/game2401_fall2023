using System.Collections;
using UnityEngine;

public class DefensiveTower : Building
{
    public Transform arrowSpawnPoint;
    public GameObject arrowPrefab;
    public float arrowSpeed = 5f;
    private Vector3 colliderSize = new Vector3(7f, 7f, 1f);
    public LayerMask targetLayer; // A camada dos Units
    private Unit affectedUnit;

    void Start()
    {
        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider>();
        }

        SetColliderSize(boxCollider, colliderSize);
        boxCollider.isTrigger = false;

        Debug.Log($"Collider Size: {boxCollider.size}, Collider Center: {boxCollider.center}");
        Debug.DrawRay(arrowSpawnPoint.position, arrowSpawnPoint.forward * 10f, Color.red, 10f);
        StartCoroutine(AttackRoutine());
    }
    void Update()
    {
        Debug.DrawRay(arrowSpawnPoint.position, arrowSpawnPoint.forward * 10f, Color.red, 0.1f);

    }

    public void SpammingRaycasts()
    {
        AttackRoutine();
    }

    private void SetColliderSize(BoxCollider collider, Vector3 size)
    {
        collider.size = size;
        collider.center = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the tag "Unit"
        if (other.CompareTag("Unit"))
        {
            Debug.Log("Collided");
            // Get the movement component of the unit (adjust as needed)
            affectedUnit = other.GetComponent<Unit>();

            // Check if the object has the movement component
            if (affectedUnit != null)
            {
                AttackRoutine();
            }
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            // Dispara raios em várias direções ao redor da torre
            int numRays = 100; // Você pode ajustar isso para aumentar ou diminuir o número de direções
            for (int i = 0; i < numRays; i++)
            {
                float angle = i * 360f / numRays;

                // Ajuste a distância para começar fora da torre (pode precisar de ajustes)
                float distance = 1.5f;

                Vector3 rayOrigin = transform.position + Vector3.up * distance;
                Vector3 direction = Quaternion.Euler(angle, 0f, 0f) * Vector3.forward;

                Ray ray = new Ray(rayOrigin, direction);
                RaycastHit hit;

                // Verifica se o raio atinge algum objeto na camada dos Units
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayer))
                {
                    // Se atingir, instancie uma flecha
                    Debug.Log($"Raycast hit: {hit.collider.gameObject.name}");

                    GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
                    Debug.Log("Flecha Spawnada");
                    Vector3 arrowDirection = (hit.point - transform.position).normalized;
                    arrow.GetComponent<Rigidbody>().velocity = arrowDirection * arrowSpeed;

                    // Aguarde um segundo antes de verificar a colisão da flecha com a unidade
                    yield return new WaitForSeconds(1f);

                    // Destrua a flecha
                    Destroy(arrow);
                    Debug.Log("Flecha Destruida");
                }
                else
                {
                    Debug.Log("Raycast did not hit any target.");
                }

                // Visualize o raio no jogo usando Debug.DrawLine
                Debug.DrawLine(ray.origin, hit.point, Color.red, 1f);
            }
        }
    }
}