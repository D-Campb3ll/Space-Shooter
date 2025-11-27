using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerups;
    [SerializeField] private GameObject _asteroidPrefab;
    [SerializeField] private GameObject _healthPickupPrefab;
    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartSpawning()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerupSpawnRoutine());
        StartCoroutine(AsteroidSpawnRoutine());
        StartCoroutine(HealthSpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EnemySpawnRoutine()
    {
        yield return new WaitForSeconds(4);
        while (_stopSpawning == false)
        {
            Vector2 posToSpawn = new Vector2(Random.Range(-8f, 8f), 6.5f);

            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(6);
        }

    }

    IEnumerator PowerupSpawnRoutine()
    {
        yield return new WaitForSeconds(7);
        while (_stopSpawning == false)
        {
            Vector2 posToSpawn = new Vector2(Random.Range(-8f, 8f), 7);
            int randomPowerup = Random.Range(0, 3);

            yield return new WaitForSeconds(Random.Range(3, 8));
            GameObject newPowerup = Instantiate(_powerups[randomPowerup], posToSpawn, Quaternion.identity);

            //Debug.Log("Powerup spawned after " + spawnTime + " seconds");
        }
    }

    IEnumerator AsteroidSpawnRoutine()
    {
        yield return new WaitForSeconds(4);
        while (_stopSpawning == false)
        {
            Vector2 posToSpawn = new Vector2(Random.Range(-8f, 8f), 7);
            yield return new WaitForSeconds(Random.Range(10, 16));
            GameObject newAtseroid = Instantiate(_asteroidPrefab, posToSpawn, Quaternion.identity);
        }
    }

    IEnumerator HealthSpawnRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector2 posToSpawn = new Vector2(Random.Range(-8f, 8f), 7);
            yield return new WaitForSeconds(Random.Range(30, 36));
            GameObject newHealthPickUp = Instantiate(_healthPickupPrefab, posToSpawn, Quaternion.identity);
        }
    }

    //Controls the stop spawning variable
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
