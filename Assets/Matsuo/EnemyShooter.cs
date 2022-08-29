using System.Collections;
using UnityEngine;
using Photon.Pun;

public class EnemyShooter : EnemyBase
{
    [SerializeField, Tooltip("�ˌ��N�[���^�C��")]
    float _coolTime;

    [SerializeField, Tooltip("�}�Y���|�W�V����")]
    Transform[] _muzzle;

    [SerializeField, Tooltip("�e�v���n�u")]
    GameObject _bullet;

    [SerializeField, Tooltip("�U���T�m����")]
    float _atackDistance = 5;

    float distance;//���_�ƓG�̋���

    Vector3 _basePos;//���_�̃|�W�V����


    void Start()
    {
        
    }

    void Update()
    {
        _basePos = base.transform.position;
        distance = Vector3.Distance(this.transform.position, _basePos);
        //�U���͈͓�
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
    /// �U�������܂Ƃ�
    /// </summary>
    public override void Atack()
    {
        StartCoroutine("Shot");
    }

    /// <summary>
    /// �e�ې���
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
