using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Raiders
{
    public class UnitController : MonoBehaviour
    {
        private HashSet<IControllable> _selectedUnits = new HashSet<IControllable>();
        private PlayerInput _input;
    
        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
        }
    
        private void OnEnable()
        {
            _input.actions["Select"].performed += Select;
            //_input.actions["MultiSelectModifier"].performed += MultiSelectStart;
            //_input.actions["MultiSelectModifier"].canceled += MultiSelectEnd;
            _input.actions["SendTroops"].performed += SendTroops;
        }
    
        private void OnDisable()
        {
            _input.actions["Select"].performed -= Select;
            //_input.actions["MultiSelectModifier"].performed -= MultiSelectStart;
            //_input.actions["MultiSelectModifier"].canceled -= MultiSelectEnd;
            _input.actions["SendTroops"].performed -= SendTroops;
        }
    
        void Update()
        {
            
        }
    
        private void Select(InputAction.CallbackContext obj)
        {
            var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    
            if (Physics.Raycast(cameraRay, out var hit, float.PositiveInfinity, LayerMask.GetMask("Unit")))
            {
                _selectedUnits.Clear();
                _selectedUnits.Add(hit.collider.GetComponent<IControllable>().mainControllable);
            }
            else
            {
                _selectedUnits.Clear();
            }
        }
    
        private void SendTroops(InputAction.CallbackContext obj)
        {
            var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    
            if (Physics.Raycast(cameraRay, out var hit, float.PositiveInfinity, LayerMask.GetMask("Terrain")))
            {
                foreach (var unit in _selectedUnits)
                {
                    unit.GoTo(hit.point);
                }
            }
        }
    
    }
}