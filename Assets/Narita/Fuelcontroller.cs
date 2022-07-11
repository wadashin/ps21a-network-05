using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuelcontroller : MonoBehaviour
{
    /// <summary>”R—¿‚Ì’l</summary>
    [SerializeField,Tooltip("”R—¿‚Ì’l")]
    private int _fuel = 0;
    /// <summary>player‚ÌFuelhold‚Ìî•ñ</summary>
    Fuelhold player = null;
    private void Start()
    {
        player = GameObject.Find("Player1(Clone)").GetComponent<Fuelhold>();
        Debug.Log($"_fuel: {_fuel}");
    }
    /// <summary>player‚É”R—¿‚Ì’l‚ğ“n‚·</summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.Holdfuel += _fuel;
            Debug.Log("player‚ª‚Â”R—¿‚Ì’l‚ª" + player.Holdfuel + "‚É‚È‚Á‚½");
            Destroy(this.gameObject);
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        //collision.gameObject.GetComponent<PlayerState>().GetDamage(_atk);

    //        player.Holdfuel += fuel;
    //        Debug.Log("player‚ª‚Â”R—¿‚Ì’l‚ª" + player.Holdfuel + "‚É‚È‚Á‚½");
    //        Destroy(this.gameObject);
    //    }
    //}
}
