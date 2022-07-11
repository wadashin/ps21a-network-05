using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField, Tooltip("�ړ����x")]
    float _moveSpeed;
    [SerializeField, Tooltip("�ő�̗�")]
    int _maxHp;
    [SerializeField, Tooltip("���ݑ̗�")]
    int _hp;
    [SerializeField, Tooltip("�U����")]
    int _atk;
    [SerializeField, Tooltip("�h���b�v�A�C�e���̎�ށi�z��̗v�f�j")]
    int _dropType;
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
        Move();
    }

    /// <summary> �G�l�~�[�ړ����� </summary>
    private void Move()
    {
        if(_base != null)
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
            //���ɋ��_�̃_���[�W����������X�N���v�g���w��
            //collision.gameObject.GetComponent<PlayerState>().GetDamage(_atk);
        }
    }

    /// <summary> �G�l�~�[���S���� </summary>
    private void Death()
    {
        if(_drop != null)
        {
            Instantiate(_drop[_dropType]);
        }
        Destroy(this);
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
