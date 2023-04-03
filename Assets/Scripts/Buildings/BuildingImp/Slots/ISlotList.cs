using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
    public interface ISlotList
    {
        public event Action AllSlotsEmpty;
        public event Action<List<Slot>> ExtraSlotsChanged;
        public event Action<List<Slot>> DefaultSlotsChanged;
        public Side OccupyingSide { get; }
        public float GeneralProgress { get; }
        public bool IsBlocked { get; }
        public int ExtraSlotsCount { get; }
        public IReadOnlySlotList ReadOnlySlotList { get; }

        public void Update();
        public bool UnblockSlot();
        public void BlockSlot(Side blockingSide);
        public bool EmptySlot();
        public bool FillSlot();
        public void AddExtraSlot();
        public bool RemoveExtraSlot();
    }
}
