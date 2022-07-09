using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuelhold : MonoBehaviour
{
    /// <summary>playerが取った燃料の値</summary>
    private int holdfuel = 0;

    public int Holdfuel { get => holdfuel; set => holdfuel = value; }
    /// <summary>BasementControllerにholdfuelの値を渡す</summary>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Fuelport"))
        {
            //BasementControllerのfuelにholdfuelを補充する
        }
    }
}
