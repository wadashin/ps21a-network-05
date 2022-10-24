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
    [Tooltip("��{�̐l��")]
    [SerializeField] int _defaultMenberNum = 4;

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
