using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField, Tooltip("移動速度")]
    float _moveSpeed;
    [SerializeField, Tooltip("最大体力")]
    int _maxHp;
    [SerializeField, Tooltip("現在体力")]
    int _hp;
    [SerializeField, Tooltip("攻撃力")]
    int _atk;
    [SerializeField, Tooltip("ドロップアイテムの種類（配列の要素）")]
    int _dropType;
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
        Move();
    }

    /// <summary> エネミー移動処理 </summary>
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
            //↓に拠点のダメージ処理があるスクリプトを指定
            //collision.gameObject.GetComponent<PlayerState>().GetDamage(_atk);
        }
    }

    /// <summary> エネミー死亡処理 </summary>
    private void Death()
    {
        if(_drop != null)
        {
            Instantiate(_drop[_dropType]);
        }
        Destroy(this);
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
