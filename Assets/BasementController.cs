using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

/// <summary>
/// 基地を制御するコンポーネント
/// Cinemachine の Dolly Cart に沿って動く
/// fuel がなくなったら止まる
/// </summary>
public class BasementController : MonoBehaviour
{
    /// <summary>動く速さ</summary>
    [SerializeField] float _speed = 2f;
    /// <summary>残りの燃料</summary>
    [SerializeField] float _fuel = 100f;
    [SerializeField] float _life = 100f;
    /// <summary>燃料が減る速さ</summary>
    [SerializeField] float _fuelReductionSpeed = 1f;
    /// <summary>燃料を表示する UI</summary>
    [SerializeField] Text _fuelText;
    [SerializeField] Slider _hpbar = null;
    PhotonView _view;
    CinemachineDollyCart _cart;
    float _lastFuel;

    void Start()
    {
        _view = GetComponent<PhotonView>();
        
        if (_view.IsMine)
        {
            _cart = FindObjectOfType<CinemachineDollyCart>();
        }
    }

    void Update()
    {
        if (!_view.IsMine) return;

        _fuel -= _fuelReductionSpeed * Time.deltaTime;

        if (_fuel > 0)
        {
            _cart.m_Position += _speed * Time.deltaTime;
        }
        else
        {
            _fuel = 0;
        }

        if (Mathf.Abs(_fuel - _lastFuel) > Mathf.Epsilon)
        {
            object[] parameters = { _fuel };
            _view.RPC(nameof(UpdateFuel), RpcTarget.All, parameters);
        }

        _lastFuel = _fuel;
    }

    [PunRPC]
    void UpdateFuel(float fuel)
    {
        _fuelText.text = fuel.ToString("F2");
    }
}
