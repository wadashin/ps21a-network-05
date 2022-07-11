using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpownManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] float _spawnTime= 1;
    [SerializeField] Transform[] _spawPoint;
    void Start()
    {
        StartCoroutine("EnemySpawnCoroutine");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnemySpawn()
    {
        int random = Random.Range(0,_spawPoint.Length);
        Instantiate(_enemyPrefab, _spawPoint[random].transform.position, Quaternion.identity);
    }

    IEnumerator EnemySpawnCoroutine()
    {
        EnemySpawn();

        yield return new WaitForSeconds(_spawnTime);

        StartCoroutine("EnemySpawnCoroutine");
    }


}
