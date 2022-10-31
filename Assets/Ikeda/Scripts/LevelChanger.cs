using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Փx�̕ύX������N���X
/// </summary>
public class LevelChanger : MonoBehaviour
{
    public static LevelChanger Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<LevelChanger>();
                if (!_instance)
                {
                    Debug.LogWarning($"�V�[�����{nameof(LevelChanger)}�����Q�[���I�u�W�F�N�g��������܂���ł����B");
                }
            }
            return _instance;
        }
    }
    static LevelChanger _instance;

    [Tooltip("�Q�����̐l��")]
    [SerializeField] int _menberNum = 0;
    [Tooltip("�E�F�[�u�̃f�[�^������z��")]
    [SerializeField] WaveData[] _waveData = default;
    [Tooltip("�E�F�[�u�}�l�[�W���[")]
    [SerializeField] WaveManager _waveManager = default;
    [Tooltip("�l�����Ƃ̍ő�o�����̔{��")]
    [SerializeField] float[] _amountFactor = { 0.2f, 0.45f, 0.7f, 1f };
    [Tooltip("�l�����Ƃ̏o���Ԋu�̔{��")]
    [SerializeField] float[] _spawnTimeFactor = { 3f, 2.5f, 1.5f, 1f };
    [SerializeField]
    LevelFactor[] _levelFactors =
    {
        new(0.2f,3f),
        new(0.45f,2.5f),
        new(0.7f,1.5f),
        new(1f,1f)

    };

    /// <summary>�Q�����̐l��</summary>
    public int MenberNum
    {
        get => _menberNum;
        set
        {
            _menberNum = value;
            WaveSet();
        }
    }

    private void OnValidate()
    {

        WaveSet();
    }

    void WaveSet()
    {
        WaveData[] copy = (WaveData[])_waveData.Clone();
        int index = Mathf.Min(_levelFactors.Length, _menberNum);
        for (int i = 0; i < copy.Length; i++)
        {
            copy[i].MaxAmount = Mathf.CeilToInt(copy[i].MaxAmount * _levelFactors[index].Amount);
            copy[i].SpawnTime *= Mathf.CeilToInt(_levelFactors[index].SpawnTime);
        }
        _waveManager.WaveData = copy;
    }


    // Start is called before the first frame update
    void Start()
    {
        WaveSet();
    }

    // Update is called once per frame
    void Update()
    {

    }

    [System.Serializable]
    struct LevelFactor
    {
        public LevelFactor(float amount, float spawnTime)
        {
            Amount = amount;
            SpawnTime = spawnTime;
        }
        public float Amount;
        public float SpawnTime;
    }
}

