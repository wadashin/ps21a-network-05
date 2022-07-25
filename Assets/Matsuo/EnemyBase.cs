using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyBase : MonoBehaviour
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
    Rigidbody _rb = default;
    GameObject _base = default; //���_�I�u�W�F�N�g
    public GameObject Base
    {
        get { return _base; }
        set { _base = value; }
    }

    private void Awake()
    {
        _base = GameObject.FindGameObjectWithTag("Base");

    }
    void Start()
    {

    }

    private void Update()
    {
        if (_base != null)
        {
            _base = GameObject.FindGameObjectWithTag("Base");
        }
    }

    /// <summary> �G�l�~�[�ړ����� </summary>
      public void Move()
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

    /// <summary>
    /// �U������
    /// </summary>
     public virtual void Atack()
    {

    }


    /// <summary>
    /// ���_�̃_���[�W�֐��Ăяo������
    /// </summary>
    /// <param name="collider"></param>
    public void CallDamage(Collision collider)
    {
        //���ɋ��_�̃_���[�W����������X�N���v�g���w��
        if (collider.gameObject.GetComponent<BaseController>())
        {
            collider.gameObject.GetComponent<BaseController>().GetDamage(_atk);
        }
    }

    /// <summary> �G�l�~�[���S���� </summary>
    private void Death()
    {
        if (_drop[_dropType] != null)
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

        if (_hp <= 0)
        {
            Death();
        }
    }
}
