using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTower : Building
{
    
    public GameObject spikePrefab; // Spike prefab to be instantiated
    public AudioSource audioSource; // Audio source to play the sound effect
    public AudioClip spawnSound; // Sound effect to play when a spike is spawned
    public float spawnLength = 10f; // Length at which spikes are spawned

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpikeShoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3[] spikePositions = new Vector3[(int)spawnLength];
            for (int i = 0; i < spawnLength; i++)
            {
                spikePositions[i] = new Vector3(i, 0, 0);
            }
            SpawnSpikes(spikePositions);


    }   }
       public void SpawnSpikes(Vector3[] spikePositions)
       {
        foreach (Vector3 position in spikePositions)
        {
            SpawnSpike(position);
        }
       }
    private void SpawnSpike(Vector3 position)
    {
        // Instantiate the spike prefab at the specified position.
        GameObject spikePrefab = Resources.Load<GameObject>("SpikePrefab");
        GameObject spike = Instantiate(spikePrefab, position, Quaternion.identity);
        // Play the spawn sound effect
        audioSource.PlayOneShot(spawnSound);

    }













}
    
       
  










  
     