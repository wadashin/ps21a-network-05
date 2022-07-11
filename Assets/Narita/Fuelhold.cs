using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuelhold : MonoBehaviour
{
    /// <summary>player��������R���̒l</summary>
    [Tooltip("�v���C���[��������R���̒l")]
    [SerializeField] public int _hold = 0;
    [SerializeField,Tooltip("base���Z�b�g")]
    BaseController _base = null;
    [SerializeField]
    GameObject fuel = null;
    public int Holdfuel {
        get
        {
            Debug.Log($"Get _hold: {_hold}");
            return _hold;
        }
        set 
        { 
            _hold = value;
            Debug.Log($"Set _hold: {_hold}"); 
        }
    }
    private void Start()
    {
        Instantiate(fuel, new Vector3(0, 0, 0), Quaternion.identity);
        Debug.Log(Holdfuel);

    }
    /// <summary>BasementController��holdfuel�̒l��n��</summary>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Port"))
        {
            _base._Fuel += this.Holdfuel;
            Debug.Log(" �񕜂��ċ��_�̔R���� " + _base._Fuel + " �ɂȂ����I");
            _hold = 0;
        }
    }
}
