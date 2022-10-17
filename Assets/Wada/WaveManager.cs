using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class WaveManager : MonoBehaviour
{
    public static List<SpawnObj> spawnObjs = new List<SpawnObj>();

    //[Tooltip("生成する敵プレハブ")]
    //[SerializeField] Enemy _enemy;

    //[Tooltip("敵を生成する間隔")]
    //[SerializeField] float _spawnTime = 1;

    //[Tooltip("とりあえずのウェーブとインターバルの長さ(今後デザイン予定)")]
    //[SerializeField] float setTime = 10;

    //[Tooltip("ウェーブとインターバルの長さを交互に入れる配列")]
    //[SerializeField] float[] _waveArray = default;

    [Tooltip("ウェーブのデータを入れる配列")]
    [SerializeField] WaveData[] _waveData = default;
    int _waveIndex = 0;
    [SerializeField] SpawnData _spawnData;


    //float _time = 0;//ウェーブ開始時はこいつをゼロにする

    /// <summary>有効なエネミーのリスト</summary>
    List<Enemy> _activeEnemys = new List<Enemy>();


    /// <summary> _spawnTimeのプロパティ  </summary>
    public float SpawnTime
    {
        get { return _waveData[_waveIndex].SpawnTime; }
        set { _waveData[_waveIndex].SpawnTime = value; }
    }

    /// <summary>ウェーブのデータを入れる配列</summary>
    public WaveData[] WaveData { get => _waveData; set => _waveData = value; }

    private void Start()
    {
        //time = setTime;
        //WaveStart();
        _spawnData = _waveData;
        _waveData = _spawnData;
        
    }

    public void WaveStart()
    {
        StartCoroutine(EnemySpawn());
        StartCoroutine(WaveFacilitator());
    }
    public void IntervalStart()
    {
        StopCoroutine(EnemySpawn());
    }

    public void ChangeTurn()
    {
        if (_waveIndex % 2 == 0)
        {
            IntervalStart();
        }
        else
        {
            WaveStart();
        }
    }

    IEnumerator WaveFacilitator()
    {
        if (!(_waveData.Length > 0))
        {
            yield break;
        }
        while (true)
        {
            yield return new WaitForSeconds(_waveData[_waveIndex].WaveTime);
            if (_waveIndex < _waveData.Length - 1)
            {
                _waveIndex++;
                Debug.Log("進");
            }
        }
    }

    /// <summary>各敵生成地点の長さに応じてランダムな場所に敵を生成</summary>
    IEnumerator EnemySpawn()
    {
        if (!(_waveData.Length > 0))
        {
            yield break;
        }
        float time = 0;
        for (; ; )
        {
            if (spawnObjs.Count > 0)
            {
                ActiveEnemysNullRemove();
                while (EnemyAmountCheck() && time > _waveData[_waveIndex].SpawnTime)
                {
                    time -= _waveData[_waveIndex].SpawnTime;
                    float allRange = spawnObjs.Sum(x => Mathf.Abs(x.Range));
                    float randomRange = Random.Range(0, allRange);
                    for (int i = 0; i < spawnObjs.Count; i++)
                    {
                        randomRange -= Mathf.Abs(spawnObjs[i].Range);
                        if (randomRange <= 0)
                        {
                            Enemy enemy = EnemySelect();
                            if (enemy)
                            {
                                GameObject go = spawnObjs[i].SpawnEnemy(enemy.name);
                                if (go.TryGetComponent<Enemy>(out Enemy e))
                                {
                                    _activeEnemys.Add(e);
                                }
                            }
                            break;
                        }
                    }
                }
            }
            time += Time.deltaTime;
            yield return null;
        }
    }

    void ActiveEnemysNullRemove()
    {
        for (int i = 0; i < _activeEnemys.Count; i++)
        {
            if (!_activeEnemys[i])
            {
                _activeEnemys.RemoveAt(i);
                i--;
            }
        }
    }


    bool EnemyAmountCheck()
    {
        if (_activeEnemys.Count < _waveData[_waveIndex].MaxAmount)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 出現させるエネミーを抽選する
    /// </summary>
    /// <returns></returns>
    Enemy EnemySelect()
    {
        int all = _waveData[_waveIndex].EnemySets.Sum(e => e.Rate);

        for (int i = 0; i < _waveData[_waveIndex].EnemySets.Length; i++)
        {
            all -= _waveData[_waveIndex].EnemySets[i].Rate;
            if (all <= 0)
            {
                return _waveData[_waveIndex].EnemySets[i].Enemy;
            }
        }

        return null;
    }


    IEnumerator CountDown()
    {
        for (int i = 0; i < 3; i++)
        {

            yield return new WaitForSeconds(1);
        }
    }
}

/// <summary>
/// ウェーブのデータ
/// </summary>
[System.Serializable]
public struct WaveData
{
    [Tooltip("ウェーブの続く時間")]
    public float WaveTime;
    [Tooltip("同時に出現する最大数")]
    public int MaxAmount;
    [Tooltip("わく頻度")]
    public float SpawnTime;
    [Tooltip("敵の種類とその比率")]
    public EnemySet[] EnemySets;

}

/// <summary>
/// エネミーの種類と比率
/// </summary>
[System.Serializable]
public class EnemySet
{
    [Tooltip("敵")]
    public Enemy Enemy;
    [Tooltip("湧く比率")]
    public int Rate;
}