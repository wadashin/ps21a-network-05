using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    Slider _slider;

    [SerializeField] float _maxValue = 100;

    [SerializeField] float _currentValue;
    void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue = _maxValue;
        _currentValue = _maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        _slider.value = _currentValue;
    }
}
