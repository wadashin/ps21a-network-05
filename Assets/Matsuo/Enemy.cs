using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]

//エネミーテスト用スクリプト
public class Enemy : MonoBehaviour
{
    [SerializeField, Header("移動速度")]
    float _moveSpeed = 1;
    [SerializeField, Header("最大体力")]
    int _maxHp = 100;
    [SerializeField, Header("現在体力")]
    int _hp = 0;
    [SerializeField, Header("攻撃力")]
    int _atk = 1;
    bool _canMove = true;
    [SerializeField, Header("ドロップアイテムの種類（配列の要素）")]
    int _dropType = 0;
    [SerializeField, Header("ドロップアイテムの配列")]
    GameObject[] _drop = default;

    [SerializeField, Header("拠点オブジェクト(確認用設定しなくてよい)")]
    GameObject _base = default; //拠点オブジェクト
    Rigidbody _rb = default;
    PhotonView _view;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _view = GetComponent<PhotonView>();
        _base = GameObject.FindGameObjectWithTag("Base");
        if( _base != null )
        {
            Debug.Log("ベースない");
        }

    }

    private void Update()
    {
        if (!_base)
        {
            _base = GameObject.FindGameObjectWithTag("Base");
            Debug.Log("ベースあった");

        }
        Move();

    }

    /// <summary> エネミー移動処理 </summary>
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
        //対象がベースの場合のみダメージ処理
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
        collider.gameObject.GetComponent<BaseController>().GetDamage(_atk);

        //↓に拠点のダメージ処理があるスクリプトを指定
        //if (collider.gameObject.GetComponent<BaseController>().GetDamage())
        //{
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

    [PunRPC]
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

    public void CallGetDamage(int damage)
    {
        _view.RPC(nameof(GetDamage), RpcTarget.All,damage);

    }
}
