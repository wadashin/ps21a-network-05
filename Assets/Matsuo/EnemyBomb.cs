using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : EnemyBase
{
    [SerializeField, Tooltip("爆発エフェクト")]
    GameObject _bombEf;

    private void Update()
    {
        Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Base")
        {
            Instantiate(_bombEf);
            Destroy(_bombEf, 1.0f);
            CallDamage(collision);
            Destroy(this, 0.5f);
        }
    }
}
