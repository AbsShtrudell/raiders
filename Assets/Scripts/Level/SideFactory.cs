using System.Collections.Generic;
using Unity.Netcode;

namespace Raiders
{
    public class SideFactory
    {
        private Dictionary<BuildingType, IBuildingData> _buildingsData;

        public SideFactory(Dictionary<BuildingType, IBuildingData> buildings)
        {
            _buildingsData = buildings;
        }

        public IBuildingImp Create(BuildingType type)
        {
            IBuildingData buildingData = _buildingsData[type];

            if (buildingData == null) throw new System.Exception("Can't find data for this type");

            if (NetworkManager.Singleton.IsHost)
                return new DefaultBuildingImp(buildingData);
            else
                return new ClientBuildingImp(buildingData);
        }
    }
}