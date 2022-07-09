using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuelcontroller : MonoBehaviour
{
    /// <summary>�R���̒l</summary>
    [SerializeField]
    private int fuel = 10;
    /// <summary>player��Fuelhold�̏��</summary>
    [SerializeField]
    Fuelhold player = null;
    /// <summary>player�ɔR���̒l��n��</summary>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            player.Holdfuel += fuel;
            Destroy(this.gameObject);
        }
    }
}
