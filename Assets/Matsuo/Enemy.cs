using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]

//�G�l�~�[�e�X�g�p�X�N���v�g
public class Enemy : MonoBehaviour
{
    [SerializeField, Header("�ړ����x")]
    float _moveSpeed = 1;
    [SerializeField, Header("�ő�̗�")]
    int _maxHp = 100;
    [SerializeField, Header("���ݑ̗�")]
    int _hp = 0;
    [SerializeField, Header("�U����")]
    int _atk = 1;
    bool _canMove = true;
    [SerializeField, Header("�h���b�v�A�C�e���̎�ށi�z��̗v�f�j")]
    int _dropType = 0;
    [SerializeField, Header("�h���b�v�A�C�e���̔z��")]
    GameObject[] _drop = default;

    [SerializeField, Header("���_�I�u�W�F�N�g(�m�F�p�ݒ肵�Ȃ��Ă悢)")]
    GameObject _base = default; //���_�I�u�W�F�N�g
    Rigidbody _rb = default;
    PhotonView _view;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _view = GetComponent<PhotonView>();
        _base = GameObject.FindGameObjectWithTag("Base");
        if( _base != null )
        {
            Debug.Log("�x�[�X�Ȃ�");
        }

    }

    private void Update()
    {
        if (!_base)
        {
            _base = GameObject.FindGameObjectWithTag("Base");
            Debug.Log("�x�[�X������");

        }
        Move();

    }

    /// <summary> �G�l�~�[�ړ����� </summary>
    private void Move()
    {
        if (_base)
        {
            if (_canMove)
            {
                transform.LookAt(_base.transform);
                Vector3 sub = _base.transform.position - transform.position;
                sub.Normalize();
                //transform.position += sub * _moveSpeed * Time.deltaTime;
                Vector3 velocity = sub.normalized * _moveSpeed;
                velocity.y = _rb.velocity.y;
                _rb.velocity = velocity;
            }

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        //�Ώۂ��x�[�X�̏ꍇ�̂݃_���[�W����
        if (collision.gameObject.tag == "Base")
        {
            CallDamage(collision);
        }

    }

    /// <summary>
    /// ���_�̃_���[�W�֐��Ăяo������
    /// </summary>
    /// <param name="collider"></param>
    void CallDamage(Collision collider)
    {
        collider.gameObject.GetComponent<BaseController>().GetDamage(_atk);

        //���ɋ��_�̃_���[�W����������X�N���v�g���w��
        //if (collider.gameObject.GetComponent<BaseController>().GetDamage())
        //{
        //}
    }

    /// <summary> �G�l�~�[���S���� </summary>
    private void Death()
    {
        if(_drop[_dropType] != null)
        {
            Instantiate(_drop[_dropType]);
        }
        //Destroy(this);
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    /// <summary> �G�l�~�[�_���[�W���� </summary>
    public void GetDamage(int damage)
    {
        _hp -= damage;
        Debug.Log(damage + " �_���[�W���󂯂ăG�l�~�[��HP�� " + _hp + " �ɂȂ����I");

        if(_hp <= 0)
        {
            Death();
        }
    }

    public void CallGetDamage(int damage)
    {
        _view.RPC(nameof(GetDamage), RpcTarget.All,damage);

    }
}
