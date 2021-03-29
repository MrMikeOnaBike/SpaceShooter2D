using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float _spawnRate = 5.0f;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 positionToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0f);

            GameObject newEnemy = Instantiate(_enemyPrefab, positionToSpawn, Quaternion.identity);

            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(_spawnRate);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
