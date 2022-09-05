using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [Tooltip("�J�n����" + nameof(GameManager.InitializeGame) + "���ĂԂ�")]
    [SerializeField] bool _isCallInitializeGame = false;
    [Tooltip("�J�n����" + nameof(WaveManager.WaveStart) + "���ĂԂ�")]
    [SerializeField] bool _isCallWaveStart = false;
    #region IOnEventCallback �̎���
    void IOnEventCallback.OnEvent(EventData photonEvent)
    {
        // �Q�[���X�^�[�g�� 1 �Ƃ���
        if (photonEvent.Code == 1)
        {
            if (_isCallInitializeGame)
            {
                GameManager gm = FindObjectOfType<GameManager>();
                if (gm)
                {
                    gm.InitializeGame();
                }
            }

            if (_isCallWaveStart)
            {
                WaveManager wm = FindObjectOfType<WaveManager>();
                if (wm)
                {
                    wm.WaveStart();
                }
            }
        }
    }
    #endregion
}
