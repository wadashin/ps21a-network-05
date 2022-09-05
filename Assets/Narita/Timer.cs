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
    GameManager gameManager = null;
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        if(!gameManager)
        {
            Debug.LogError("�^�C�}�[������ɓ����܂���");
        }
    }
    public void TimerMethod()
    {
        if (gameManager.Gamestart)
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
}
