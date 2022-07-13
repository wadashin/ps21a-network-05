using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WaveManager : MonoBehaviour
{
    public static List<SpawnObj> spawnObjs = new List<SpawnObj>();

    [SerializeField] GameObject _enemy;


    void Start()
    {
        StartCoroutine(A());
    }

    void Update()
    {

    }

    IEnumerator A()
    {
        for (; ; )
        {
            if (spawnObjs.Count > 0)
            {
                spawnObjs[Random.Range(0, spawnObjs.Count)].SpawnEnemy(_enemy);
            }
            yield return new WaitForSeconds(1);
        }
    }
}
