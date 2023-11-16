using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;

    [SerializeField] private GameObject[] _spawnPoints;
    [SerializeField] private GameObject _enemy;

    private float _spawnTimer = 2f;
    private float _spawnRateIncrease = 5f;


    void Start()
    {
        StartCoroutine(SpawnNextEnemy());
        StartCoroutine(SpawnRateIncrease());
    }

    IEnumerator SpawnNextEnemy()
    {
        int nextSpawnLocation = Random.Range(0, _spawnPoints.Length);

        Instantiate(_enemy, _spawnPoints[nextSpawnLocation].transform.position, Quaternion.identity);
        yield return new WaitForSeconds(_spawnTimer);
        if (!_gameManager._gameOver)
        {
            StartCoroutine(SpawnNextEnemy());
        }

        yield return null;
    }

    IEnumerator SpawnRateIncrease()
    {

        yield return new WaitForSeconds(_spawnRateIncrease);
        if (_spawnTimer >= 0.5f)
        {
            _spawnTimer -= 0.2f;
        }
        if (!_gameManager._gameOver)
        {
            StartCoroutine(SpawnRateIncrease());
        }
    }
}
