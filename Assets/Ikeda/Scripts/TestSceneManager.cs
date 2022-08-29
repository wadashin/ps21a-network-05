using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    #region IOnEventCallback の実装
    void IOnEventCallback.OnEvent(EventData photonEvent)
    {
        // ゲームスタートを 1 とする
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
