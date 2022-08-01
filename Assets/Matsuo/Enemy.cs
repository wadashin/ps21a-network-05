using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]


public class Enemy : MonoBehaviour
{
    [SerializeField, Tooltip("�ړ����x")]
    float _moveSpeed = 1;
    [SerializeField, Tooltip("�ő�̗�")]
    int _maxHp = 100;
    [SerializeField, Tooltip("���ݑ̗�")]
    int _hp = 0;
    [SerializeField, Tooltip("�U����")]
    int _atk = 1;
    [SerializeField, Tooltip("�h���b�v�A�C�e���̎�ށi�z��̗v�f�j")]
    int _dropType = 0;
    [SerializeField, Tooltip("�h���b�v�A�C�e���̔z��")]
    GameObject[] _drop = default;
    GameObject _base = default; //���_�I�u�W�F�N�g
    Rigidbody _rb = default;

    void Start()
    {
        _base = GameObject.FindGameObjectWithTag("Base");

    }

    private void Update()
    {
        if (_base != null)
        {
            _base = GameObject.FindGameObjectWithTag("Base");
        }
        Move();

    }

    /// <summary> �G�l�~�[�ړ����� </summary>
    private void Move()
    {
        if (_base != null)
        {
            transform.LookAt(_base.transform);
            Vector3 sub = _base.transform.position - transform.position;
            sub.Normalize();
            transform.position += sub * _moveSpeed * Time.deltaTime;
            Vector3 velocity = sub.normalized * _moveSpeed;
            velocity.y = _rb.velocity.y;
            _rb.velocity = velocity;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
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
        //���ɋ��_�̃_���[�W����������X�N���v�g���w��
        //if(collider.gameObject.GetComponent<>().GetDamage())
        //{
        //    //collider.gameObject.GetComponent<>().GetDamage(_atk);
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
}
