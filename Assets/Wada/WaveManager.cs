using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class WaveManager : MonoBehaviour
{
    public static List<SpawnObj> spawnObjs = new List<SpawnObj>();

    [Tooltip("��������G�v���n�u")]
    [SerializeField] Enemy _enemy;

    [Tooltip("�G�𐶐�����Ԋu")]
    [SerializeField] float _spawnTime = 1;

    [Tooltip("�Ƃ肠�����̃E�F�[�u�ƃC���^�[�o���̒���(����f�U�C���\��)")]
    [SerializeField] float setTime = 10;

    float time = 0;//�E�F�[�u�J�n���͂������[���ɂ���
    

    /// <summary> _spawnTime�̃v���p�e�B  </summary>
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


    /// <summary>�e�G�����n�_�̒����ɉ����ă����_���ȏꏊ�ɓG�𐶐�</summary>
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
