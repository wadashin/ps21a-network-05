using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [Tooltip("開始時に" + nameof(GameManager.InitializeGame) + "を呼ぶか")]
    [SerializeField] bool _isCallInitializeGame = false;
    [Tooltip("開始時に" + nameof(WaveManager.WaveStart) + "を呼ぶか")]
    [SerializeField] bool _isCallWaveStart = false;
    #region IOnEventCallback の実装
    void IOnEventCallback.OnEvent(EventData photonEvent)
    {
        // ゲームスタートを 1 とする
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
