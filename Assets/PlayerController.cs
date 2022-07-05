using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

/// <summary>
/// プレイヤーを操作するコンポーネント
/// クリックした場所に移動する
/// </summary>
[RequireComponent(typeof(PhotonView), typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    /// <summary>移動速度</summary>
    [SerializeField] float _speed = 6f;
    /// <summary>クリック可能なレイヤー</summary>
    [SerializeField] LayerMask _layerMask;
    /// <summary>移動のためのレイキャストの距離</summary>
    [SerializeField] float _raycastDistance = 20f;
    /// <summary>ストップする目的地からの距離</summary>
    [SerializeField] float _stoppingDistance = 1f;
    PhotonView _view;
    Rigidbody _rb;
    /// <summary>目的地の座標</summary>
    Vector3 _destination;

    void Start()
    {
        _view = GetComponent<PhotonView>();
        _rb = GetComponent<Rigidbody>();
        _destination = transform.position;

        if (_view.IsMine)
        {
            // カメラをセットアップする
            var vcam = FindObjectOfType<CinemachineVirtualCameraBase>();
            vcam.LookAt = transform;
            vcam.Follow = transform;

            // マスタークライアントからゲーム開始イベントを fire する
            if (PhotonNetwork.IsMasterClient)
            {
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
                raiseEventOptions.Receivers = ReceiverGroup.All;
                SendOptions sendOptions = new SendOptions();
                PhotonNetwork.RaiseEvent(1, null, raiseEventOptions, sendOptions);
            }
        }
    }

    void Update()
    {
        if (!_view.IsMine) return;

        if (Input.GetButton("Fire2"))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, _raycastDistance, _layerMask))
            {
                _destination = hit.point;
            }
        }

        if (Vector3.Distance(transform.position, _destination) < _stoppingDistance)
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
        }
        else
        {
            Vector3 dir = _destination - transform.position;
            dir.y = 0;
            dir = dir.normalized * _speed;
            _rb.velocity = new Vector3(dir.x, _rb.velocity.y, dir.z);
        }
    }
}
