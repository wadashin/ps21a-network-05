using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スライダーを動かす処理
/// </summary>

public class SliderScript : MonoBehaviour
{
    Slider _slider;
  
    /// <summary>最大値</summary>
    [SerializeField] float _maxValue = 100;
    /// <summary>最小値</summary>
    [SerializeField] float _minValue = 0;
    /// <summary>現在値</summary>
    [SerializeField] float _currentValue;

    void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue = _maxValue;
        _slider.minValue = _minValue;
    }

    // Update is called once per frame
    void Update()
    {
        _slider.value = _currentValue;
    }
}
