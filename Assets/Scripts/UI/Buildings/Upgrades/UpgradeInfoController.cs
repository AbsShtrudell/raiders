using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Raiders
{
    public class UpgradeInfoController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI slotsAmount;
        [SerializeField]
        private TextMeshProUGUI defense;
        [SerializeField]
        private TextMeshProUGUI attack;
        [SerializeField]
        private TextMeshProUGUI upkeep;
        [SerializeField]
        private TextMeshProUGUI cost;

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show(IBuildingData data)
        {
            slotsAmount.text = data.SquadSlots.ToString();
            defense.text = data.DefenseMultyplier.ToString();
            attack.text = data.SquadTypeInfo.Damage.ToString();
            upkeep.text = (data.Income - data.Upkeep).ToString();
            cost.text = data.Cost.ToString();

            gameObject.SetActive(true);
        }
    }
}
