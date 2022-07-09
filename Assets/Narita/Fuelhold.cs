using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuelhold : MonoBehaviour
{
    /// <summary>player‚ªæ‚Á‚½”R—¿‚Ì’l</summary>
    private int holdfuel = 0;

    public int Holdfuel { get => holdfuel; set => holdfuel = value; }
    /// <summary>BasementController‚Éholdfuel‚Ì’l‚ğ“n‚·</summary>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Fuelport"))
        {
            //BasementController‚Ìfuel‚Éholdfuel‚ğ•â[‚·‚é
        }
    }
}
