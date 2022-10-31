using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 難易度の変更をするクラス
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
                    Debug.LogWarning($"シーン上に{nameof(LevelChanger)}を持つゲームオブジェクトが見つかりませんでした。");
                }
            }
            return _instance;
        }
    }
    static LevelChanger _instance;

    [Tooltip("参加中の人数")]
    [SerializeField] int _menberNum = 0;
    [Tooltip("ウェーブのデータを入れる配列")]
    [SerializeField] WaveData[] _waveData = default;
    [Tooltip("ウェーブマネージャー")]
    [SerializeField] WaveManager _waveManager = default;
    [Tooltip("人数ごとの最大出現数の倍率")]
    [SerializeField] float[] _amountFactor = { 0.2f, 0.45f, 0.7f, 1f };
    [Tooltip("人数ごとの出現間隔の倍率")]
    [SerializeField] float[] _spawnTimeFactor = { 3f, 2.5f, 1.5f, 1f };
    [SerializeField]
    LevelFactor[] _levelFactors =
    {
        new(0.2f,3f),
        new(0.45f,2.5f),
        new(0.7f,1.5f),
        new(1f,1f)

    };

    /// <summary>参加中の人数</summary>
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

