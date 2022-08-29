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
    //追加↓成田担当
    private bool start = false;
    public bool Start { get => start; set => start = value; }

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
        }
    }

    #region IOnEventCallback の実装
    void IOnEventCallback.OnEvent(EventData photonEvent)
    {
        // ゲームスタートを 1 とする
        if (photonEvent.Code == 1)
        {
            InitializeGame();
            start = true;
        }
    }
    #endregion
}
