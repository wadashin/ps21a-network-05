using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuelhold : MonoBehaviour
{
    /// <summary>playerが取った燃料の値</summary>
    [Tooltip("プレイヤーが取った燃料の値")]
    [SerializeField] public int _hold = 0;
    [SerializeField,Tooltip("baseをセット")]
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
    /// <summary>BasementControllerにholdfuelの値を渡す</summary>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Port"))
        {
            _base._Fuel += this.Holdfuel;
            Debug.Log(" 回復して拠点の燃料が " + _base._Fuel + " になった！");
            _hold = 0;
        }
    }
}
