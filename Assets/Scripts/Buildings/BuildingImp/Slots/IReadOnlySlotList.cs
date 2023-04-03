using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raiders
{
    public interface IReadOnlySlotList
    {
        public event Action AllSlotsEmpty;
        public event Action<List<Slot>> ExtraSlotsChanged;
        public event Action<List<Slot>> DefaultSlotsChanged;
        public Side OccupyingSide { get; }
        public float GeneralProgress { get; }
        public bool IsBlocked { get; }
        public int ExtraSlotsCount { get; }
    }
}
