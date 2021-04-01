using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float _enemySpawnRate = 5.0f;
    [SerializeField] private float _minSpawnRate = 5.0f;
    [SerializeField] private float _maxSpawnRate = 10.0f;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject _tripleShotPowerup;
    [SerializeField] private GameObject _speedBoostPowerup;
    [SerializeField] private GameObject _shieldsPowerup;
    [SerializeField] private bool _stopSpawning = false;

    private float _minPosX = -8f;
    private float _maxPosX = 8f;
    private float _posY = 7f;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(LowerSpawnRate());
    }

    IEnumerator LowerSpawnRate()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(10.0f);

            _enemySpawnRate -= 0.1f;

            if(_enemySpawnRate < 1.0f)
            {
                _enemySpawnRate = 1.0f;
            }
        }
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 positionToSpawn = new Vector3(Random.Range(_minPosX, _maxPosX), _posY, 0f);

            GameObject newEnemy = Instantiate(_enemyPrefab, positionToSpawn, Quaternion.identity);

            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(_enemySpawnRate);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(10f);

        while (_stopSpawning == false)
        {
            //decide which powerup to spawn
            int powerupID = Random.Range(0, 3);
            Vector3 positionToSpawn = new Vector3(Random.Range(_minPosX, _maxPosX), _posY, 0f);
            float powerupSpawnRate = Random.Range(_minSpawnRate, _maxSpawnRate);

            switch (powerupID)
            {
                case 0: // triple shot
                    Instantiate(_tripleShotPowerup, positionToSpawn, Quaternion.identity);
                    break;
                case 1: // speed boost
                    Instantiate(_speedBoostPowerup, positionToSpawn, Quaternion.identity);
                    break;
                case 2: // shields
                    Instantiate(_shieldsPowerup, positionToSpawn, Quaternion.identity);
                    break;
                default:
                    //do nothing
                    break;
            }

            yield return new WaitForSeconds(powerupSpawnRate);
        }
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
