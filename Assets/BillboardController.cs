using UnityEngine;

/// <summary>
/// UI をカメラ方向に向けるコンポーネント
/// Canvas または UI オブジェクトに追加する
/// </summary>
public class BillboardController : MonoBehaviour
{

    void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
