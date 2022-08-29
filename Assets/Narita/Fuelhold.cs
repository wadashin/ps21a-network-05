using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuelhold : MonoBehaviour
{
    /// <summary>player��������R���̒l</summary>
    [Tooltip("�v���C���[��������R���̒l")]
    [SerializeField] public int _hold = 0;
    /// <summary>�R��</summary>
    //[SerializeField] GameObject fuel = null;
    /// <summary>Base�̐e</summary>
 �@�@GameObject _baseparent = null;
    /// <summary>Base</summary>
    BaseController _base = null;
    /// <summary>_baseparent�̎q�I�u�W�F�N�g����</summary>
    private int baseparentchildcount = 1;
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
        if (GameObject.FindGameObjectWithTag("BaseParent"))
        {
            _baseparent = GameObject.FindGameObjectWithTag("BaseParent");
            Debug.Log(Holdfuel);
        }
        else
        {
            Debug.LogError("BaseParent�̃^�O�����Y��Ă��邩�ABase�̐e�I�u�W�F�N�g�����݂��܂���");
            //Instantiate(fuel, new Vector3(0, 0, 0), Quaternion.identity)
        }
    }
    /// <summary>Start��Base���̂��m�ۏo���Ȃ����߂����Ŋm�ۂ���</summary>
    private void Update()
    {
        if (!cheak)
        {
            if (_baseparent.transform.childCount == baseparentchildcount)
            {
                _base = _baseparent.GetComponentInChildren<BaseController>();
                Debug.Log(_baseparent);
                cheak = true;
            }
        }
    }
    /// <summary>BasementController��holdfuel�̒l��n��</summary>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Port"))
        {
            if (_hold <= 0)
            {
                Debug.Log("�R���������ĂȂ���");
            }
            else
            {
                _base._Fuel += _hold;
                if (_base._Fuel > _base._MaxFuel)
                {
                    _base._Fuel = _base._MaxFuel;
                }
                Debug.Log(" �񕜂��ċ��_�̔R���� " + _base._Fuel + " �ɂȂ����I");
                _hold = 0;
            }
        }
    }
}
