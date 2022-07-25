using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スライダーを動かす処理
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
    /// Baseの位置をスライダーと同期させて動かす処理
    /// </summary>
    /// <param name="distance">道のり,DollyTrackのPath Lengthの値</param>
    /// <param name="currentValue">Baseの地点,DollyCartのposition</param>
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
