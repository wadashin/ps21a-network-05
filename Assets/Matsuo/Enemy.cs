using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]


public class Enemy : MonoBehaviour
{
    [SerializeField, Tooltip("移動速度")]
    float _moveSpeed = 1;
    [SerializeField, Tooltip("最大体力")]
    int _maxHp = 100;
    [SerializeField, Tooltip("現在体力")]
    int _hp = 0;
    [SerializeField, Tooltip("攻撃力")]
    int _atk = 1;
    [SerializeField, Tooltip("ドロップアイテムの種類（配列の要素）")]
    int _dropType = 0;
    [SerializeField, Tooltip("ドロップアイテムの配列")]
    GameObject[] _drop = default;
    GameObject _base = default; //拠点オブジェクト
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

    /// <summary> エネミー移動処理 </summary>
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
    /// 拠点のダメージ関数呼び出し処理
    /// </summary>
    /// <param name="collider"></param>
    void CallDamage(Collision collider)
    {
        //↓に拠点のダメージ処理があるスクリプトを指定
        //if(collider.gameObject.GetComponent<>().GetDamage())
        //{
        //    //collider.gameObject.GetComponent<>().GetDamage(_atk);
        //}
    }

    /// <summary> エネミー死亡処理 </summary>
    private void Death()
    {
        if(_drop[_dropType] != null)
        {
            Instantiate(_drop[_dropType]);
        }
        //Destroy(this);
        PhotonNetwork.Destroy(gameObject);
    }

    /// <summary> エネミーダメージ処理 </summary>
    public void GetDamage(int damage)
    {
        _hp -= damage;
        Debug.Log(damage + " ダメージを受けてエネミーのHPが " + _hp + " になった！");

        if(_hp <= 0)
        {
            Death();
        }
    }
}
