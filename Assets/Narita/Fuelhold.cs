using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuelhold : MonoBehaviour
{
    /// <summary>player��������R���̒l</summary>
    [Tooltip("�v���C���[��������R���̒l")]
    [SerializeField] public int _hold = 0;
    /// <summary>�R��</summary>
    [SerializeField] GameObject fuel = null;
    /// <summary>Base�̐e</summary>
 �@�@GameObject _baseparent = null;
    /// <summary>Base</summary>
    BaseController _base = null;
    bool cheak = false;
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
        _baseparent = GameObject.FindGameObjectWithTag("Base");
        Instantiate(fuel, new Vector3(0, 0, 0), Quaternion.identity);
        Debug.Log(Holdfuel);
    }
    /// <summary>Start��Base���̂��m�ۏo���Ȃ����߂����Ŋm�ۂ���</summary>
    private void Update()
    {
        if (!cheak)
        {
            if (_baseparent.transform.childCount == 1)
            {
                _base = _baseparent.GetComponentInChildren<BaseController>();
                cheak = true;
            }
        }
    }
    /// <summary>BasementController��holdfuel�̒l��n��</summary>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Port"))
        {
            _base._Fuel += _hold;
            Debug.Log(" �񕜂��ċ��_�̔R���� " + _base._Fuel + " �ɂȂ����I");
            _hold = 0;
        }
    }
}
