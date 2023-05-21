using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Raiders
{
    public class AddresUIController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField ipInput;
        [SerializeField] private TMP_InputField portInput;

        public event System.Action<string, int> onApply;
        public event System.Action onBack;

        public void Apply()
        {
            onApply?.Invoke(ipInput.text, Convert.ToInt32(portInput.text));
        }

        public void Back()
        {
            onBack.Invoke();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            ipInput.text = null;
            portInput.text = null;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
