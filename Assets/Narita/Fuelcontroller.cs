using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuelcontroller : MonoBehaviour
{
    /// <summary>”R—¿‚Ì’l</summary>
    [SerializeField]
    private int fuel = 10;
    /// <summary>player‚ÌFuelhold‚Ìî•ñ</summary>
    [SerializeField]
    Fuelhold player = null;
    /// <summary>player‚É”R—¿‚Ì’l‚ğ“n‚·</summary>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            player.Holdfuel += fuel;
            Destroy(this.gameObject);
        }
    }
}
