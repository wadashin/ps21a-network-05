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
    [Tooltip("基本の人数")]
    [SerializeField] int _defaultMenberNum = 4;

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
        for (int i = 0; i < copy.Length; i++)
        {
            copy[i].MaxAmount = (int)(copy[i].MaxAmount * (_menberNum / (float)_defaultMenberNum));
            copy[i].SpawnTime *= (float)_defaultMenberNum / _menberNum;
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
}
