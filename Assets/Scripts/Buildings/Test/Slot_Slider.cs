using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot_Slider : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Slider _slider;

    public void SetValue(float value)
    {
        _slider.value = value;
        _text.text = value.ToString();
    }

    public void SetBlockState(bool value)
    {
        if(value)
            _text.color = Color.red;
        else
            _text.color = Color.white;
    }
}
