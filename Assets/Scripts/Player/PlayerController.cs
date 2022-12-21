using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Side _side = Side.Vikings;

    private List<Building> _selectedBuildings = new List<Building>();
    private Building _lastSelectedBuilding;

    private PlayerInput _input;

    private bool _multiSelect = false;

    public Side Side => _side;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _input.actions["Select"].performed += Select;
        _input.actions["MultiSelectModifier"].performed += MultiSelectStart;
        _input.actions["MultiSelectModifier"].canceled += MultiSelectEnd;
        _input.actions["SendTroops"].performed += SendTroops;
    }

    private void OnDisable()
    {
        _input.actions["Select"].performed -= Select;
        _input.actions["MultiSelectModifier"].performed -= MultiSelectStart;
        _input.actions["MultiSelectModifier"].canceled -= MultiSelectEnd;
        _input.actions["SendTroops"].performed -= SendTroops;
    }

    private void MultiSelectStart(InputAction.CallbackContext obj)
    {
        _multiSelect = true;
    }

    private void MultiSelectEnd(InputAction.CallbackContext obj)
    {
        _multiSelect = false;
    }

    private void SendTroops(InputAction.CallbackContext obj)
    {
        RaycastHit hit;
        var building = RaycastBuilding(out hit);

        if (building)
        {
            foreach(var b in _selectedBuildings)
            {
                if (b != building)
                {
                    b.SendTroops(building);
                }
            }

            DeselectAll();
        }
    }

    private void Select(InputAction.CallbackContext obj)
    {
        RaycastHit hit;
        var building = RaycastBuilding(out hit);

        if (_multiSelect)
        {
            if (building && building.Side == _side)
            {
                MultiSelection(building);
            }
        }
        else
        {
            if (building && building.Side == _side)
            {
                SingleSelection(building);
            }
            else
            {
                DeselectAll();
            }
        }
    }

    private void SingleSelection(Building building)
    {
        foreach (var b in _selectedBuildings)
        {
            b.Deselect();
        }
        _selectedBuildings.Clear();

        _selectedBuildings.Add(building);

        _lastSelectedBuilding = building;

        building.Select();
    }

    private void MultiSelection(Building building)
    {
        _selectedBuildings.Add(building);

        _lastSelectedBuilding = building;

        building.Select();
    }

    private void DeselectAll()
    {
        foreach (var b in _selectedBuildings)
        {
            b.Deselect();
        }
        _selectedBuildings.Clear();

        _lastSelectedBuilding = null;
    }


    private Building RaycastBuilding(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Building")))
        {
            return hit.collider.GetComponent<Building>();
        }
        return null;
    }
}
