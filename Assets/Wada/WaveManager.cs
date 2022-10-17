using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class WaveManager : MonoBehaviour
{
    public static List<SpawnObj> spawnObjs = new List<SpawnObj>();

    //[Tooltip("��������G�v���n�u")]
    //[SerializeField] Enemy _enemy;

    //[Tooltip("�G�𐶐�����Ԋu")]
    //[SerializeField] float _spawnTime = 1;

    //[Tooltip("�Ƃ肠�����̃E�F�[�u�ƃC���^�[�o���̒���(����f�U�C���\��)")]
    //[SerializeField] float setTime = 10;

    //[Tooltip("�E�F�[�u�ƃC���^�[�o���̒��������݂ɓ����z��")]
    //[SerializeField] float[] _waveArray = default;

    [Tooltip("�E�F�[�u�̃f�[�^������z��")]
    [SerializeField] WaveData[] _waveData = default;
    int _waveIndex = 0;
    [SerializeField] SpawnData _spawnData;


    //float _time = 0;//�E�F�[�u�J�n���͂������[���ɂ���

    /// <summary>�L���ȃG�l�~�[�̃��X�g</summary>
    List<Enemy> _activeEnemys = new List<Enemy>();


    /// <summary> _spawnTime�̃v���p�e�B  </summary>
    public float SpawnTime
    {
        get { return _waveData[_waveIndex].SpawnTime; }
        set { _waveData[_waveIndex].SpawnTime = value; }
    }

    /// <summary>�E�F�[�u�̃f�[�^������z��</summary>
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
                Debug.Log("�i");
            }
        }
    }

    /// <summary>�e�G�����n�_�̒����ɉ����ă����_���ȏꏊ�ɓG�𐶐�</summary>
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
    /// �o��������G�l�~�[�𒊑I����
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
/// �E�F�[�u�̃f�[�^
/// </summary>
[System.Serializable]
public struct WaveData
{
    [Tooltip("�E�F�[�u�̑�������")]
    public float WaveTime;
    [Tooltip("�����ɏo������ő吔")]
    public int MaxAmount;
    [Tooltip("�킭�p�x")]
    public float SpawnTime;
    [Tooltip("�G�̎�ނƂ��̔䗦")]
    public EnemySet[] EnemySets;

}

/// <summary>
/// �G�l�~�[�̎�ނƔ䗦
/// </summary>
[System.Serializable]
public class EnemySet
{
    [Tooltip("�G")]
    public Enemy Enemy;
    [Tooltip("�N���䗦")]
    public int Rate;
}