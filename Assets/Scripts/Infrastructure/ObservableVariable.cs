using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders.Infrastructure
{
    [Serializable]
    public class ObservableVariable<T>
    {
        [SerializeField]
        private T _value;
        public T Value { 
            get 
            { 
                return _value; 
            } 
            set 
            {
                OnValueChanged?.Invoke(_value, value); 
                _value = value; 
            } 
        }

        public event System.Action<T,T> OnValueChanged;
    }
}
