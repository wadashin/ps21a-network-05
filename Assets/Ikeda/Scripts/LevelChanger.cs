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

    /// <summary>�Q�����̐l��</summary>
    public int MenberNum { get => _menberNum; set => _menberNum = value; }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
