using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeSpawner : MonoBehaviour
{
    public GameObject spikePrefab;
    public float spawnInterval = 0.3f; 
    [SerializeField] private int spawnCount = 5; 
    public Vector3 spawnOffset = new Vector3(1f, 0f, 0f); 

    private void Start()
    {
        StartCoroutine(SpawnSpikes());
    }

    private IEnumerator SpawnSpikes()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject newSpike = Instantiate(spikePrefab, spawnPosition + new Vector3(spawnOffset.x * (i + 1), 0, 0), Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

}