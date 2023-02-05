using System.Collections.Generic;

namespace Raiders
{
    public class SideFactory
    {
        private Dictionary<BuildingType, IBuildingData> _buildingsData;

        public SideFactory(Dictionary<BuildingType, IBuildingData> buildings)
        {
            _buildingsData = buildings;
        }

        public BuildingImp Create(BuildingType type)
        {
            IBuildingData buildingData = _buildingsData[type];

            if (buildingData == null) throw new System.Exception("Can't find data for this type");

            return new SimpleBuildingImp(buildingData);
        }
    }
}