using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    /// <summary>分</summary>
    [SerializeField]
    int minute = 0;
    ///<summary>秒</summary>
    [SerializeField]
    float second = 0;
    ///<summary>表示用テキスト</summary>
    [SerializeField]
    Text timertext = null;
    ///<summary>GameManagerが付いているオブジェクト名</summary>
    [SerializeField]
    string objectname = "GameManagerが付いているオブジェクト名";
    /////<summary>終了しているかどうかの判定用</summary>
    //bool finish = false;

    GameManager gamemanager = null;
    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.Find(objectname).GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        second += Time.deltaTime;
        if (second >= 10f)
        {
            minute++;
            second = second - 10;
        }
        timertext.text = minute.ToString("00") + ":" + Mathf.Floor(second).ToString("00");
    }
}
