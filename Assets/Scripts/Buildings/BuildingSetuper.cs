using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Raiders
{
    public class BuildingSetuper : MonoBehaviour
    {
        [Inject]
        private BuildingImpCreator _buildingImpCreator;
        [Inject]
        private SlotsControllerCreator _slotsContrCreator;

        [SerializeField]
        private Building _building;
        [Space]
        [SerializeField]
        private BuildingType _type = BuildingType.Simple;
        [SerializeField]
        private Side _side = Side.Rebels;

        private void Awake()
        {
            if (_building == null)
                _building = GetComponent<Building>();

            SetUpBuilding(_building);   
        }

        private void SetUpBuilding(Building building)
        {
            if (building == null) return;

            building.Init(_side, _type, _buildingImpCreator, _slotsContrCreator);
        }
    }
}
