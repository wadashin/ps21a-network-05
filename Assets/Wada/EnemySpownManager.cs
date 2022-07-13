using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpownManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Tooltip("プレハブ化したエネミー")]
    [SerializeField] GameObject _enemyPrefab;
    [Tooltip("敵が生成される間隔")]
    [SerializeField] float _spawnTime = 1;
    [Tooltip("敵が生成される座標の配列")]
    [SerializeField] Transform[] _spawPoint = default;
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

    public void SpawnCoroutineStart()
    {

    }

    IEnumerator EnemySpawnCoroutine()
    {
        EnemySpawn();

        yield return new WaitForSeconds(_spawnTime);

        StartCoroutine("EnemySpawnCoroutine");
    }


}
