using System.Collections.Generic;

namespace Raiders
{
    public class BuildingImpCreator
    {
        private Dictionary<Side, SideFactory> _sideFactories;

        public BuildingImpCreator(Dictionary<Side, SideFactory> sideFactories)
        {
            _sideFactories = sideFactories;
        }

        public IBuildingImp Create(BuildingType type, Side side)
        {
            _sideFactories.TryGetValue(side, out SideFactory sideFactory);

            if (sideFactory == null) throw new System.Exception(string.Format("Can't find Side Factory for {0} side", side));

            return sideFactory.Create(type);
        }
    }
}
