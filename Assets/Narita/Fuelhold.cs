using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuelhold : MonoBehaviour
{
    /// <summary>playerが取った燃料の値</summary>
    [Tooltip("プレイヤーが取った燃料の値")]
    [SerializeField] public int _hold = 0;
    /// <summary>燃料</summary>
    //[SerializeField] GameObject fuel = null;
    /// <summary>Baseの親</summary>
 　　GameObject _baseparent = null;
    /// <summary>Base</summary>
    BaseController _base = null;
    /// <summary>_baseparentの子オブジェクト総数</summary>
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
            Debug.LogError("BaseParentのタグをつけ忘れているか、Baseの親オブジェクトが存在しません");
            //Instantiate(fuel, new Vector3(0, 0, 0), Quaternion.identity)
        }
    }
    /// <summary>StartでBase自体を確保出来ないためここで確保する</summary>
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
    /// <summary>BasementControllerにholdfuelの値を渡す</summary>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Port"))
        {
            if (_hold <= 0)
            {
                Debug.Log("燃料を持ってないよ");
            }
            else
            {
                _base._Fuel += _hold;
                if (_base._Fuel > _base._MaxFuel)
                {
                    _base._Fuel = _base._MaxFuel;
                }
                Debug.Log(" 回復して拠点の燃料が " + _base._Fuel + " になった！");
                _hold = 0;
            }
        }
    }
}
