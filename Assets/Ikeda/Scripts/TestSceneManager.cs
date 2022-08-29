using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    #region IOnEventCallback �̎���
    void IOnEventCallback.OnEvent(EventData photonEvent)
    {
        // �Q�[���X�^�[�g�� 1 �Ƃ���
        if (photonEvent.Code == 1)
        {
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm)
            {
                gm.InitializeGame();
            }
        }
    }
    #endregion
}
