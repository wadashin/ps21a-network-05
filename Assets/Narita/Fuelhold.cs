using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuelhold : MonoBehaviour
{
    /// <summary>player��������R���̒l</summary>
    private int holdfuel = 0;

    public int Holdfuel { get => holdfuel; set => holdfuel = value; }
    /// <summary>BasementController��holdfuel�̒l��n��</summary>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Fuelport"))
        {
            //BasementController��fuel��holdfuel���[����
        }
    }
}
