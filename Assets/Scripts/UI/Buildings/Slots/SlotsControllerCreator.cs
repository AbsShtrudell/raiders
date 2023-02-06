using System.Collections.Generic;
using Zenject;

namespace Raiders
{
    public class SlotsControllerCreator
    {
        private Dictionary<Side, ISlotsControllerFactory> slotControllerFactories;

        public SlotsControllerCreator(Dictionary<Side, ISlotsControllerFactory> slotControllerFactories)
        {
            this.slotControllerFactories = slotControllerFactories;
        }

        public SlotsController Create(Side side, IBuildingImp imp)
        {
            ISlotsControllerFactory factory;

            slotControllerFactories.TryGetValue(side, out factory);

            if (factory == null) throw new System.Exception(string.Format("Can't find SlotsController for {0} side", side));

            return factory.Construct(imp);
        }
    }
}
