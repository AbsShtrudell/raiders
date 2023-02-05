using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raiders
{
    public interface ISlotsControllerFactory
    {
        public SlotsController Construct(BuildingImp imp);
    }
}
