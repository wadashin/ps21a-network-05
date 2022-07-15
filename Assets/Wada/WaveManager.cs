using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class WaveManager : MonoBehaviour
{
    public static List<SpawnObj> spawnObjs = new List<SpawnObj>();

    [Tooltip("生成する敵プレハブ")]
    [SerializeField] Enemy _enemy;

    [Tooltip("敵を生成する間隔")]
    [SerializeField] float _spawnTime = 1;

    void Start()
    {
        StartCoroutine(A());
    }

    private void Update()
    {
        
    }

    /// <summary>各敵生成地点の長さに応じてランダムな場所に敵を生成</summary>
    IEnumerator A()
    {
        for (; ; )
        {
            if (spawnObjs.Count > 0)
            {
                float allRange = spawnObjs.Sum(x => Mathf.Abs(x.Range));
                float randomRange =  Random.Range(0, allRange);
                for(int i = 0; i < spawnObjs.Count; i++)
                {
                    randomRange -= Mathf.Abs(spawnObjs[i].Range);
                    if (randomRange <= 0)
                    {
                        spawnObjs[i].SpawnEnemy(_enemy);
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(_spawnTime);
        }
    }
}
