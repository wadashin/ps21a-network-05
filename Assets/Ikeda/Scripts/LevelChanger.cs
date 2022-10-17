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

    /// <summary>参加中の人数</summary>
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
