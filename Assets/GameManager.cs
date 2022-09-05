using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

/// <summary>
/// ゲームを管理するコンポーネント
/// ゲームが始まったら基地を生成する
/// </summary>
public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    /// <summary>基地のプレハブ名</summary>
    [SerializeField] string _basePrefabName = "Base";
    /// <summary>DollyCart のオブジェクト</summary>
    [SerializeField] Transform _dollyCart;
    private bool gamestart = false;

    public bool Gamestart { get => gamestart; set => gamestart = value; }

    void Update()
    {

    }

    public void InitializeGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var go = PhotonNetwork.Instantiate(_basePrefabName, Vector3.zero, Quaternion.identity);
            go.transform.SetParent(_dollyCart);
            go.transform.localPosition = Vector3.zero;
            gamestart = true;
        }
    }

    #region IOnEventCallback の実装
    void IOnEventCallback.OnEvent(EventData photonEvent)
    {
        // ゲームスタートを 1 とする
        //if (photonEvent.Code == 1)
        //{
        //    InitializeGame();
        //}
    }
    #endregion
}
