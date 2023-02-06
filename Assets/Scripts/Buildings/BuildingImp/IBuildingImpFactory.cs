using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
    public interface IBuildingImpFactory
    {
        public IBuildingImp Create(BuildingData data);
    }
}
