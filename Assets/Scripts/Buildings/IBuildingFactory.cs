using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
    public interface IBuildingFactory
    {
        public Building Create(BuildingType type, Side side);
    }
}
