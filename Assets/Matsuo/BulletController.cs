using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour
{
    [SerializeField, Tooltip("スピード")] float _speed = 15f;
    [SerializeField, Tooltip("破棄するまでの時間")] float _lifeTime = 3f;
    [SerializeField, Tooltip("攻撃力")] public int _atk = 10;
    [SerializeField, Tooltip("エフェクト用")]  GameObject explosionPrefab;


    private void Start()
    {
        Bullet();

    }
    void Update()
    {
    }

    void Bullet()
    {
        Rigidbody _rb = GetComponent<Rigidbody>();
        Vector3 directionToTarget;
        Destroy(this.gameObject, _lifeTime);
        GameObject targetObject = GameObject.FindWithTag("Base");
        if (targetObject)
        {
            directionToTarget = (targetObject.transform.position - transform.position).normalized;
            _rb.velocity = directionToTarget * 30;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Base")
        {
            collision.gameObject.GetComponent<Enemy>().GetDamage(_atk);
            //Destroy(this.gameObject);
            //GameObject effect = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
            //Destroy(effect, 1.0f);
        };
    }

}
