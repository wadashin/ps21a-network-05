using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuelcontroller : MonoBehaviour
{
    /// <summary>�R���̒l</summary>
    [SerializeField,Tooltip("�R���̒l")]
    private int _fuel = 0;
    /// <summary>player��Fuelhold�̏��</summary>
    Fuelhold player = null;
    private void Start()
    {
        player = GameObject.Find("Player1(Clone)").GetComponent<Fuelhold>();
        Debug.Log($"_fuel: {_fuel}");
    }
    /// <summary>player�ɔR���̒l��n��</summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.Holdfuel += _fuel;
            Debug.Log("player�����R���̒l��" + player.Holdfuel + "�ɂȂ���");
            Destroy(this.gameObject);
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        //collision.gameObject.GetComponent<PlayerState>().GetDamage(_atk);

    //        player.Holdfuel += fuel;
    //        Debug.Log("player�����R���̒l��" + player.Holdfuel + "�ɂȂ���");
    //        Destroy(this.gameObject);
    //    }
    //}
}
