
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
    private float _maxfuel = 100f;
    /// <summary>�c��̔R��</summary>
    [SerializeField] public float _fuel = 100f;
    /// <summary>�c��̃��C�t</summary>
    [SerializeField] private float _life = 100f;
    /// <summary>��������</summary>
    [SerializeField] float _speed = 2f;
    /// <summary>�R�������鑬��</summary>
    [SerializeField] float _fuelReductionSpeed = 1f;
    /// <summary>�R����\������ UI</summary>
    [SerializeField] Text _fuelText;
    [SerializeField] Slider _hpbar = null;
    PhotonView _view;
    CinemachineDollyCart _cart;
    /// <summary>_fuel������</summary>
    float _lastFuel;

    public float _Fuel { get => _fuel; set => _fuel = value; }
    void Start()
    {
        _hpbar.maxValue = _life;
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
        else if(_fuel > _maxfuel)//�v���C���[�����[�����R����base�̎��R���̍ő�l�𒴂����ꍇ�B
        {
            _fuel = _maxfuel;
        }
        else
        {
            _fuel = 0;
        }

        if (Mathf.Abs(_fuel - _lastFuel) > Mathf.Epsilon)
        {
            object[] parameters = { _Fuel };
            _view.RPC(nameof(UpdateFuel), RpcTarget.All, parameters);
        }

        _lastFuel = _fuel;
    }

    [PunRPC]
    void UpdateFuel(float fuel)
    {
        _fuelText.text = fuel.ToString("F2");
    }

    public void GetDamage(int damage)
    {
        _hpbar.maxValue -= damage;
        Debug.Log(damage + " �_���[�W���󂯂ċ��_��HP�� " + _life + " �ɂȂ����I");

        if (_life <= 0)
        {
            Death();
        }
    }

    private void Death()
    {

    }
}
