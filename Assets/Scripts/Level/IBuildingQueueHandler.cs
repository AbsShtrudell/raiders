using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raiders
{
    public interface IBuildingQueueHandler
    {
        public void Upgrade(IBuildingData buildingData, bool free, Building notifyer);
        public Graphs.Path<Building> GetPath(Building target, Building notifyer);
        public void SquadSent(Building destination, SquadTypeInfo squadTypeInfo, Building notifyer);
    }
}
