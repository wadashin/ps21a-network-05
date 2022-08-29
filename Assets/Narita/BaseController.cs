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
public class BaseController : MonoBehaviour
{
    /// <summary>燃料の最大値</summary>
    private float _maxfuel = 0;
    private float _maxlife = 0;
    /// <summary>残りの燃料</summary>
    [SerializeField] public float _fuel = 100f;
    /// <summary>残りのライフ</summary>
    [SerializeField] private float _life = 100f;
    /// <summary>動く速さ</summary>
    [SerializeField] float _speed = 2f;
    /// <summary>燃料が減る速さ</summary>
    [SerializeField] float _fuelReductionSpeed = 1f;
    /// <summary>燃料を表示する UI</summary>
    [SerializeField]
    Slider _fuelbar = null;
    /// <summary>体力を表示する UI</summary>
    [SerializeField]
    Slider _hpbar = null;
    PhotonView _view;
    CinemachineDollyCart _cart;
    /// <summary>変化後の_fuelを持つ</summary>
    float _lastFuel;
    /// <summary>変化後の_lifeを持つ</summary>
    float _lastLife;

    ///// <summary>ダメージ確認用変数</summary>
    //int DemoDamage = 5;

    public float _Fuel { get => _fuel; set => _fuel = value; }
    public float _MaxFuel { get => _maxfuel; set => _maxfuel = value; }
    void Start()
    {

        _view = GetComponent<PhotonView>();

        _maxfuel = _fuel;
        _maxlife = _life;

        //_fuelbar = GameObject.Find("Fuelbar").GetComponent<Slider>();
        //_hpbar = GameObject.Find("Hpbar").GetComponent<Slider>();

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
        else if (_fuel > _maxfuel)//プレイヤーから補充した燃料がbaseの持つ燃料の最大値を超えた場合。
        {
            _fuel = _maxfuel;
        }
        else
        {
            _fuel = 0;
        }

        if (Mathf.Abs(_fuel - _lastFuel) > Mathf.Epsilon)//Mathf.Epsilonは0ではない最も小さな値
        {
            object[] parameters = { _Fuel };
            _view.RPC(nameof(UpdateFuel), RpcTarget.All, parameters);
        }

        //if (Mathf.Abs(_life - _lastLife) > Mathf.Epsilon)
        //{
        //    object[] parameters = { _life };
        //    _view.RPC(nameof(UpdateLife), RpcTarget.All, parameters);
        //}
        _fuelbar.value = _fuel / _maxfuel;
        _lastFuel = _fuel;
    }

    public void GetDamage(int damage)
    {
        _life -= damage;
        _lastLife = _life;

        _hpbar.value = _life / _maxlife;
        Debug.Log(damage + " ダメージを受けて拠点のHPが " + _life + " になった！");
        if (_life <= 0)
        {
            Death();
        }
    }

    private void Death()
    {

    }

    [PunRPC]
    void UpdateFuel(float fuel)
    {
        _fuelbar.value = fuel;
    }

    //void UpdateLife(float life)
    //{
    //    _hpbar.value = life;
    //}


}
