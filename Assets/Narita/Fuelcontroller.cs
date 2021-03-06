using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuelcontroller : MonoBehaviour
{
    /// <summary>燃料の値</summary>
    [SerializeField,Tooltip("燃料の値")]
    private int _fuel = 0;
    /// <summary>playerのFuelholdの情報</summary>
    Fuelhold player = null;
    private void Start()
    {
        Debug.Log($"_fuel: {_fuel}");
    }
    /// <summary>playerに燃料の値を渡す</summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<Fuelhold>();
            player.Holdfuel += _fuel;
            Debug.Log("playerが持つ燃料の値が" + player.Holdfuel + "になった");
            Destroy(this.gameObject);
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        //collision.gameObject.GetComponent<PlayerState>().GetDamage(_atk);

    //        player.Holdfuel += fuel;
    //        Debug.Log("playerが持つ燃料の値が" + player.Holdfuel + "になった");
    //        Destroy(this.gameObject);
    //    }
    //}
}
