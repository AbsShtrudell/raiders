using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    [SerializeField] private BuildingData _buildingData;
    [SerializeField] private GameObject _defSlotSliderRef;
    [SerializeField] private GameObject _extrSlotSliderRef;
    [SerializeField] private Transform _slotsHolder;
    private SimpleBuildingImp _buildingImp;
    private List<Slot_Slider> _defaultSlotSliders = new List<Slot_Slider>();
    private List<Slot_Slider> _extraSlotSliders = new List<Slot_Slider>();

    private void Awake()
    {
        _buildingImp = new SimpleBuildingImp(_buildingData);
    }

    private void OnEnable()
    {
        _buildingImp.GetSlotList().DefaultSlotsChanged += OnDefaultSlotsChanged;
        _buildingImp.GetSlotList().ExtraSlotsChanged += OnExtraSlotsChanged;
    }

    private void OnDisable()
    {
        _buildingImp.GetSlotList().DefaultSlotsChanged -= OnDefaultSlotsChanged;
        _buildingImp.GetSlotList().ExtraSlotsChanged -= OnExtraSlotsChanged;
    }

    private void OnDefaultSlotsChanged(List<Slot> slotList)
    {
        if (slotList.Count > _defaultSlotSliders.Count)
            for (int i = _defaultSlotSliders.Count; i < slotList.Count; i++)
                AddSlotSlider(_defaultSlotSliders, _defSlotSliderRef);
        else if (slotList.Count < _defaultSlotSliders.Count)
            for (int i = slotList.Count; i < _defaultSlotSliders.Count; i++)
                RemoveSlotSlider(_defaultSlotSliders);

        for(int i = 0; i < _defaultSlotSliders.Count; i++)
        {
            _defaultSlotSliders[i].SetValue(slotList[i].Progress);
            _defaultSlotSliders[i].SetBlockState(slotList[i].IsBlocked);
        }
    }

    private void OnExtraSlotsChanged(List<Slot> slotList)
    {
        if (slotList.Count > _extraSlotSliders.Count)
            for (int i = _extraSlotSliders.Count; i < slotList.Count; i++)
                AddSlotSlider(_extraSlotSliders, _extrSlotSliderRef);
        else if (slotList.Count < _extraSlotSliders.Count)
            for (int i = slotList.Count; i < _extraSlotSliders.Count; i++)
                RemoveSlotSlider(_extraSlotSliders);

        for (int i = 0; i < _extraSlotSliders.Count; i++)
        {
            _extraSlotSliders[i].SetValue(slotList[i].Progress);
            _extraSlotSliders[i].SetBlockState(slotList[i].IsBlocked);
        }
    }

    private void AddSlotSlider(List<Slot_Slider> list, GameObject slot)
    {
        list.Add(GameObject.Instantiate(slot, _slotsHolder).GetComponent<Slot_Slider>());
    }

    private void RemoveSlotSlider(List<Slot_Slider> list)
    {
        var slot = list[list.Count - 1];
        list.Remove(slot);
        Destroy(slot.gameObject);
    }

    private void Update()
    {
        _buildingImp.Update();
    }

    public void Attack()
    {
        _buildingImp.Attackers();
    }

    public void Reinforcement()
    {
        _buildingImp.Reinforcement();
    }

    public void SendTroops()
    {
        _buildingImp.SendTroops();
    }
}
