using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotsController : MonoBehaviour
{
    [SerializeField] 
    private HorizontalLayoutGroup _slotsHolder;

    protected SircleSlot.Factory _defaultFactory;
    protected SircleSlot.Factory _extraFactory;

    private List<SircleSlot> _defaultSlots = new List<SircleSlot>();
    private List<SircleSlot> _extraSlots = new List<SircleSlot>();

    private void SetHolderSize(int count)
    {
        var rect = _slotsHolder.GetComponent<RectTransform>();

        float width = _defaultFactory.SlotRef.GetComponent<RectTransform>().rect.width;
        float spacing = _slotsHolder.spacing;
        rect.sizeDelta = new Vector2(width * count + spacing * (count - 1), rect.sizeDelta.y);
    }

    protected void OnDefaultSlotsChanged(List<Slot> slotList)
    {
        if (slotList.Count != _defaultSlots.Count)
            SetHolderSize(slotList.Count);

        if (slotList.Count > _defaultSlots.Count)
            for (int i = _defaultSlots.Count; i < slotList.Count; i++)
                AddSlot(_defaultSlots, _defaultFactory);
        else if (slotList.Count < _defaultSlots.Count)
            for (int i = slotList.Count; i < _defaultSlots.Count; i++)
                RemoveSlot(_defaultSlots);

        for (int i = 0; i < _defaultSlots.Count; i++)
        {
            _defaultSlots[i].SetValue(slotList[i].Progress);
            _defaultSlots[i].SetBlockState(slotList[i].IsBlocked);
        }
    }

    protected void OnExtraSlotsChanged(List<Slot> slotList)
    {
        if (slotList.Count > _extraSlots.Count)
            for (int i = _extraSlots.Count; i < slotList.Count; i++)
                AddSlot(_extraSlots, _extraFactory);
        else if (slotList.Count < _extraSlots.Count)
            for (int i = slotList.Count; i < _extraSlots.Count; i++)
                RemoveSlot(_extraSlots);

        for (int i = 0; i < _extraSlots.Count; i++)
        {
            _extraSlots[i].SetValue(slotList[i].Progress);
            _extraSlots[i].SetBlockState(slotList[i].IsBlocked);
        }
    }

    private void AddSlot(List<SircleSlot> list, SircleSlot.Factory factory)
    {
        var slot = factory.Construct();
        slot.transform.parent = _slotsHolder.transform;
        slot.transform.localScale = Vector3.one;
        list.Add(slot);
    }

    private void RemoveSlot(List<SircleSlot> list)
    {
        if (list.Count <= 0) return;

        var slot = list[list.Count - 1];

        list.Remove(slot);

        Destroy(slot.gameObject);
    }

    public class Factory
    {
        private SircleSlot.Factory _defFactory;
        private SircleSlot.Factory _extraFactory;
        private SlotsController _controllerRef;

        public Factory(SircleSlot.Factory defFactory, SircleSlot.Factory extraFactory, SlotsController controllerRef)
        {
            _defFactory = defFactory;
            _extraFactory = extraFactory;
            _controllerRef = controllerRef;
        }

        public SlotsController Construct(BuildingImp imp)
        {
            var slotsController = Instantiate(_controllerRef);

            imp.SlotList.DefaultSlotsChanged += slotsController.OnDefaultSlotsChanged;
            imp.SlotList.ExtraSlotsChanged += slotsController.OnExtraSlotsChanged;

            slotsController._defaultFactory = _defFactory;
            slotsController._extraFactory = _extraFactory;

            slotsController.GetComponent<Canvas>().worldCamera = Camera.main;

            return slotsController;
        }
    }
}
