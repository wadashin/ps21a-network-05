using System.Collections;
using UnityEngine;
using Photon.Pun;

public class EnemyShooter : EnemyBase
{
    [SerializeField, Tooltip("射撃クールタイム")]
    float _coolTime;

    [SerializeField, Tooltip("マズルポジション")]
    Transform[] _muzzle;

    [SerializeField, Tooltip("弾プレハブ")]
    GameObject _bullet;

    [SerializeField, Tooltip("攻撃探知距離")]
    float _atackDistance = 5;

    float distance;//拠点と敵の距離

    Vector3 _basePos;//拠点のポジション


    void Start()
    {
        
    }

    void Update()
    {
        _basePos = base.transform.position;
        distance = Vector3.Distance(this.transform.position, _basePos);
        //攻撃範囲内
        if (distance <= _atackDistance)
        {
            Atack();
        }
        else
        {
            Move();
        }
    }


    /// <summary>
    /// 攻撃処理まとめ
    /// </summary>
    public override void Atack()
    {
        StartCoroutine("Shot");
    }

    /// <summary>
    /// 弾丸生成
    /// </summary>
    /// <returns></returns>
    IEnumerator Shot()
    {
        transform.LookAt(base.transform);
        //PhotonNetwork.Instantiate(_bullet, GetRandomPosition(), Quaternion.identity, 0);
        var go = Instantiate(_bullet);

        go.transform.position = _muzzle[0].position;
        go.transform.forward = _muzzle[0].forward;
        yield return _coolTime;
    }
}
