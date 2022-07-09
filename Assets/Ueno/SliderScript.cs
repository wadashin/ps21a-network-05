using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �X���C�_�[�𓮂�������
/// </summary>

public class SliderScript : MonoBehaviour
{
    Slider _slider;
  
    /// <summary>�ő�l</summary>
    [SerializeField] float _maxValue = 100;
    /// <summary>�ŏ��l</summary>
    [SerializeField] float _minValue = 0;
    /// <summary>���ݒl</summary>
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
