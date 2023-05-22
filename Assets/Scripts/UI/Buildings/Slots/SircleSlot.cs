using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace Raiders
{
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
        [SerializeField]
        private Image _crossImage;

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
            _crossImage.gameObject.SetActive(value);
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
}