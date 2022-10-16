using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SircleSlot : MonoBehaviour
{
    [SerializeField]
    private Color _backColor;
    [SerializeField]
    private Color _forColor;
    [SerializeField]
    private Image _fillImage;
    [SerializeField]
    private Image _backImage;

    private void OnEnable()
    {
        _fillImage.color = _forColor;
        _backImage.color = _backColor;
    }

    public void SetValue(float value)
    {
        _fillImage.fillAmount = value;
    }

    public void SetBlockState(bool value)
    {

    }

    public class Factory
    {
        private SircleSlot _slotRef;

        public SircleSlot SlotRef => _slotRef;

        public Factory(SircleSlot slotRef) => _slotRef = slotRef;

        public SircleSlot Construct()
        {
            var result = Instantiate(_slotRef);
            return result;
        }
    }
}
