using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

/// <summary>
/// ��n�𐧌䂷��R���|�[�l���g
/// Cinemachine �� Dolly Cart �ɉ����ē���
/// fuel ���Ȃ��Ȃ�����~�܂�
/// </summary>
public class BaseController : MonoBehaviour
{
    /// <summary>�R���̍ő�l</summary>
    private float _maxfuel = 0;
    private float _maxlife = 0;
    /// <summary>�c��̔R��</summary>
    [SerializeField] public float _fuel = 100f;
    /// <summary>�c��̃��C�t</summary>
    [SerializeField] private float _life = 100f;
    /// <summary>��������</summary>
    [SerializeField] float _speed = 2f;
    /// <summary>�R�������鑬��</summary>
    [SerializeField] float _fuelReductionSpeed = 1f;
    /// <summary>�R����\������ UI</summary>
    [SerializeField]
    Slider _fuelbar = null;
    /// <summary>�̗͂�\������ UI</summary>
    [SerializeField]
    Slider _hpbar = null;
    PhotonView _view;
    CinemachineDollyCart _cart;
    /// <summary>�ω����_fuel������</summary>
    float _lastFuel;
    /// <summary>�ω����_life������</summary>
    float _lastLife;

    ///// <summary>�_���[�W�m�F�p�ϐ�</summary>
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
        else if (_fuel > _maxfuel)//�v���C���[�����[�����R����base�̎��R���̍ő�l�𒴂����ꍇ�B
        {
            _fuel = _maxfuel;
        }
        else
        {
            _fuel = 0;
        }

        if (Mathf.Abs(_fuel - _lastFuel) > Mathf.Epsilon)//Mathf.Epsilon��0�ł͂Ȃ��ł������Ȓl
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
        Debug.Log(damage + " �_���[�W���󂯂ċ��_��HP�� " + _life + " �ɂȂ����I");
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
