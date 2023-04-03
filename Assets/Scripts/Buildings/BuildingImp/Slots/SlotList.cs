using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Raiders
{
    public class SlotList : IReadOnlySlotList, ISlotList
    {
        private float enemySpeed = 1f;
        private float extraDecaySpeed = 1f;

        private List<Slot> _slots;
        private List<Slot> _extraSlots;

        private float _squadRecoveryTime;
        private int _blokcedSlots = 0;
        private int _size;
        private Side _occupyingSide;

        public event Action AllSlotsEmpty;
        public event Action<List<Slot>> ExtraSlotsChanged;
        public event Action<List<Slot>> DefaultSlotsChanged;

        public Side OccupyingSide => _occupyingSide;

        public float GeneralProgress
        {
            get
            {
                float output = 0f;
                foreach (Slot slot in _slots)
                {
                    output += slot.Progress;
                }
                foreach (Slot slot in _extraSlots)
                {
                    output += slot.Progress;
                }
                return output;
            }
        }
        public bool IsBlocked
        { get { return _blokcedSlots > 0; } }
        public int ExtraSlotsCount
        { get { return _extraSlots.Count; } }

        public IReadOnlySlotList ReadOnlySlotList => this;

        public SlotList(int size, float squadRecoveryTime)
        {
            _slots = new List<Slot>(size);
            for (int i = 0; i < size; i++)
            {
                _slots.Add(new Slot(0f));
            }
            _extraSlots = new List<Slot>(0);

            _size = size;
            _squadRecoveryTime = squadRecoveryTime;
        }

        public void Update()
        {
            if (_extraSlots.Count > 0)
                UpdateExtraSlots();
            else
                UpdateSlots();

            if (GeneralProgress == 0f) AllSlotsEmpty?.Invoke();
        }

        private void UpdateSlots()
        {
            if (!IsBlocked)
            {
                foreach (var slot in _slots)
                {
                    if (slot.IsFull) continue;

                    slot.Recover(_squadRecoveryTime);
                    break;
                }
            }
            else
            {
                for (int i = _slots.Count - 1; i >= 0; i--)
                {
                    if (_slots[i].IsEmpty) continue;

                    if (_slots[i].IsBlocked) _slots[i].Decay(enemySpeed);

                    break;
                }
            }

            DefaultSlotsChanged?.Invoke(_slots);
        }

        private void UpdateExtraSlots()
        {
            if (_extraSlots.Count == 0) return;

            Slot extra_slot = _extraSlots[_extraSlots.Count - 1];

            if (extra_slot.Progress == 0f)
            {
                _extraSlots.Remove(extra_slot);
            }
            else
            {
                extra_slot.Decay(extraDecaySpeed);
            }

            ExtraSlotsChanged?.Invoke(_extraSlots);
        }

        public bool UnblockSlot()
        {
            if (!IsBlocked) return false;

            foreach (var slot in _slots)
            {
                if (!slot.IsBlocked) continue;

                slot.IsBlocked = false;
                _blokcedSlots--;
                DefaultSlotsChanged?.Invoke(_slots);
                return true;
            }
            return false;
        }

        private void BlockSlot()
        {
            if (_blokcedSlots >= _size) return;

            for (int i = _slots.Count - 1; i >= 0; i--)
            {
                if (_slots[i].IsEmpty || _slots[i].IsBlocked) continue;

                _slots[i].IsBlocked = true;
                _blokcedSlots++;
                DefaultSlotsChanged?.Invoke(_slots);
                break;
            }
        }

        public void BlockSlot(Side blockingSide)
        {
            BlockSlot();
            _occupyingSide = blockingSide;
        }

        public bool EmptySlot()
        {

            if (_slots[0].IsFull && !_slots[0].IsBlocked)
            {
                _slots.RemoveAt(0);
                _slots.Add(new Slot(0f));
                DefaultSlotsChanged?.Invoke(_slots);
                return true;
            }
            else return false;
        }

        public bool FillSlot()
        {
            foreach (var slot in _slots)
            {
                if (slot.IsFull) continue;

                slot.Fill();
                DefaultSlotsChanged?.Invoke(_slots);
                return true;
            }
            return false;
        }

        public void AddExtraSlot()
        {
            _extraSlots.Add(new Slot(1f));

            if (_extraSlots.Count > 1)
            {
                Slot buf = _extraSlots[_extraSlots.Count - 2];
                _extraSlots[_extraSlots.Count - 2] = _extraSlots[_extraSlots.Count - 1];
                _extraSlots[_extraSlots.Count - 1] = buf;
            }

            ExtraSlotsChanged?.Invoke(_extraSlots);
        }

        public bool RemoveExtraSlot()
        {
            if (_extraSlots.Count == 0) return false;
            _extraSlots.Remove(_extraSlots[_extraSlots.Count - 1]);
            ExtraSlotsChanged?.Invoke(_extraSlots);
            return true;
        }
    }
}