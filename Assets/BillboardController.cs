using UnityEngine;

/// <summary>
/// UI ���J���������Ɍ�����R���|�[�l���g
/// Canvas �܂��� UI �I�u�W�F�N�g�ɒǉ�����
/// </summary>
public class BillboardController : MonoBehaviour
{

    void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
