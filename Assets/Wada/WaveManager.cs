using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class WaveManager : MonoBehaviour
{
    public static List<SpawnObj> spawnObjs = new List<SpawnObj>();

    [SerializeField] Enemy _enemy;

    [SerializeField] float _spawn = 1;

    void Start()
    {
        StartCoroutine(A());
    }

    private void Update()
    {
        
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
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
            yield return new WaitForSeconds(_spawn);
        }
    }
}
