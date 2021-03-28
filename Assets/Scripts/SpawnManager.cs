using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float _spawnRate = 5.0f;
    [SerializeField] private GameObject _enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            Vector3 positionToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0f);
            Instantiate(_enemyPrefab, positionToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(_spawnRate);
        }
    }

}
