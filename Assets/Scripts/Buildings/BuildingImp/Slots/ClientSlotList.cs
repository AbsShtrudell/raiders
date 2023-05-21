using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

namespace Raiders
{
    public class ClientSlotList : IReadOnlySlotList
    {
        public Side OccupyingSide { get; private set; }
        public float GeneralProgress { get; private set; }
        public bool IsBlocked { get; private set; }
        public List<Slot> Slots { get; private set; }
        public List<Slot> ExtraSlots { get; private set; }
        public int ExtraSlotsCount
        { get { return ExtraSlots.Count; } }

        public event Action AllSlotsEmpty;
        public event Action<List<Slot>> ExtraSlotsChanged;
        public event Action<List<Slot>> DefaultSlotsChanged;

        public void SetData(NetworkData data)
        {
            OccupyingSide = data.OccupyingSide;
            Slots = data.Slots.ToList();
            IsBlocked = data.IsBlocked;
            GeneralProgress = data.GeneralProgress;
            ExtraSlots = data.ExtraSlots.ToList();

            if (GeneralProgress == 0f) AllSlotsEmpty?.Invoke();

            DefaultSlotsChanged.Invoke(Slots);
            ExtraSlotsChanged.Invoke(ExtraSlots);
        }

        public struct NetworkData : INetworkSerializable
        {
            public Side _occupyingSide;
            public bool _isBlocked;
            public float _generalProgress;
            public Slot[] _slots;
            public Slot[] _extraSlots;

            public Side OccupyingSide => _occupyingSide;
            public bool IsBlocked => _isBlocked;
            public float GeneralProgress => _generalProgress;
            public Slot[] Slots => _slots;
            public Slot[] ExtraSlots => _extraSlots;

            public NetworkData(Side side, bool isBlocked, float generalProgress, Slot[] slots, Slot[] extraslots)
            {
                _occupyingSide = side;
                _isBlocked = isBlocked;
                _generalProgress = generalProgress;
                _slots = slots;
                _extraSlots = extraslots;
            }

            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                serializer.SerializeValue(ref _occupyingSide );
                serializer.SerializeValue(ref _isBlocked);
                serializer.SerializeValue(ref _generalProgress);

                int defaultLength = 0;
                int extraLength = 0;

                if (!serializer.IsReader)
                {
                    defaultLength = _slots.Length;
                    extraLength = _extraSlots.Length;
                }

                serializer.SerializeValue(ref defaultLength);
                serializer.SerializeValue(ref extraLength);

                if (serializer.IsReader)
                {
                    _slots = new Slot[defaultLength];
                    _extraSlots = new Slot[extraLength];
                }

                for (int n = 0; n < defaultLength; ++n)
                {
                    if (serializer.IsReader)
                        _slots[n] = new Slot();

                    _slots[n].NetworkSerialize(serializer);
                }
                for (int n = 0; n < extraLength; ++n)
                {
                    if (serializer.IsReader)
                        _extraSlots[n] = new Slot();

                    _extraSlots[n].NetworkSerialize(serializer);
                }
            }
        }
    }
}
