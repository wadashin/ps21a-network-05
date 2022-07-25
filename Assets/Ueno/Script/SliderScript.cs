using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �X���C�_�[�𓮂�������
/// </summary>

public class SliderScript : MonoBehaviour
{
    private Slider _slider;
    private bool isSettingValue = false;

    private void Start()
    {
        _slider = GetComponent<Slider>();
    }

    /// <summary>
    /// Base�̈ʒu���X���C�_�[�Ɠ��������ē���������
    /// </summary>
    /// <param name="distance">���̂�,DollyTrack��Path Length�̒l</param>
    /// <param name="currentValue">Base�̒n�_,DollyCart��position</param>
    public void UpdateSlider(float distance,float currentValue)
    {
        if (_slider != null)
        {
            if (!isSettingValue)
            {
                _slider.maxValue = distance;
                isSettingValue = true;
            }

            _slider.value = currentValue;
        
        }
    }
}
