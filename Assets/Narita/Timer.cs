using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    /// <summary>��</summary>
    [SerializeField]
    int minute = 0;
    ///<summary>�b</summary>
    [SerializeField]
    float second = 0;
    ///<summary>�\���p�e�L�X�g</summary>
    [SerializeField]
    Text timertext = null;
    ///<summary>GameManager���t���Ă���I�u�W�F�N�g��</summary>
    [SerializeField]
    string objectname = "GameManager���t���Ă���I�u�W�F�N�g��";
    /////<summary>�I�����Ă��邩�ǂ����̔���p</summary>
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
