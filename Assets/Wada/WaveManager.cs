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

    [Tooltip("とりあえずのウェーブとインターバルの長さ(今後デザイン予定)")]
    [SerializeField] float setTime = 10;

    float time = 0;//ウェーブ開始時はこいつをゼロにする
    

    /// <summary> _spawnTimeのプロパティ  </summary>
    public float SpawnTime
    {
        get { return _spawnTime; }
        set { _spawnTime = value; }
    }


    private void Start()
    {
        time = setTime;
    }
    private void Update()
    {
        if(time < setTime)
        {
            time += Time.deltaTime;
        }
    }

    public void WaveStart()
    {
        StartCoroutine(EnemySpawn());
        time = 0;
    }
    public void IntervalStart()
    {
        StopCoroutine(EnemySpawn());
        time = 0;
    }


    /// <summary>各敵生成地点の長さに応じてランダムな場所に敵を生成</summary>
    IEnumerator EnemySpawn()
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
                        spawnObjs[i].SpawnEnemy(_enemy.name);
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(_spawnTime);
        }
    }
}
