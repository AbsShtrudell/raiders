using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Raiders
{
    public class UpgradeController : MonoBehaviour
    {
        [SerializeField]
        private Button _upgradeButtonRef;
        [SerializeField]
        private Button _downgradeButtonRef;
        [SerializeField]
        private Transform _upgradeButtonsHolder;
        [SerializeField]
        private Transform _upgradeInfoPrefab;

        private List<Button> _upgradeButtons = new List<Button>();

        private UpgradeInfoController _upgradeInfoController;

        public void InitButtons(IBuildingData data, System.Action<int> action)
        {
            foreach (var button in _upgradeButtons)
            {
                Destroy(button.gameObject);
            }
            _upgradeButtons.Clear();

            if (data.PreviousLevel != null)
            {
                Button button = Instantiate(_downgradeButtonRef, _upgradeButtonsHolder);
                _upgradeButtons.Add(button);
                button.onClick.AddListener(() => action(-1));

                EventTrigger.Entry starthover = new EventTrigger.Entry();
                starthover.eventID = EventTriggerType.PointerEnter;
                starthover.callback.AddListener((eventData) => { ShowUpgradeInfo(data.PreviousLevel, button.transform); });

                EventTrigger.Entry endhover = new EventTrigger.Entry();
                endhover.eventID = EventTriggerType.PointerExit;
                endhover.callback.AddListener((eventData) => { HideUpgradeInfo(); });

                button.GetComponent<EventTrigger>().triggers.Add(starthover);
                button.GetComponent<EventTrigger>().triggers.Add(endhover);
            }

            for (int i = 0; i < data.Upgrades.Count; i++)
            {
                int j = i;
                Button button = Instantiate(_upgradeButtonRef, _upgradeButtonsHolder);
                _upgradeButtons.Add(button);
                button.onClick.AddListener(() => action(j));
                button.image.sprite = data.Upgrades[i].Icon;

                EventTrigger.Entry starthover = new EventTrigger.Entry();
                starthover.eventID = EventTriggerType.PointerEnter;
                starthover.callback.AddListener((eventData) => { ShowUpgradeInfo(data.Upgrades[j], button.transform); });

                EventTrigger.Entry endhover = new EventTrigger.Entry();
                endhover.eventID = EventTriggerType.PointerExit;
                endhover.callback.AddListener((eventData) => { HideUpgradeInfo(); });

                button.GetComponent<EventTrigger>().triggers.Add(starthover);
                button.GetComponent<EventTrigger>().triggers.Add(endhover);
            }
        }

        public void ShowUpgradeInfo(IBuildingData data, Transform notyfier)
        {
            if (_upgradeInfoController == null)
                _upgradeInfoController = Instantiate(_upgradeInfoPrefab , transform).GetComponent<UpgradeInfoController>();
            _upgradeInfoController.transform.position = notyfier.position + Vector3.up * 2;
            _upgradeInfoController.Show(data);
        }

        public void HideUpgradeInfo()
        {
            _upgradeInfoController.Hide();
        }

        public void Hide()
        {
            _upgradeButtonsHolder.gameObject.SetActive(false);
        }

        public void Show()
        {
            _upgradeButtonsHolder.gameObject.SetActive(true);
        }
    }
}
