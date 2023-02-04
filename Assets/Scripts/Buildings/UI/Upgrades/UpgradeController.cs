using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{
    [SerializeField]
    private Button _upgradeButtonRef;
    [SerializeField]
    private Button _downgradeButtonRef;
    [SerializeField]
    private Transform _upgradeButtonsHolder;

    private List<Button> _upgradeButtons = new List<Button>();

    public void InitButtons(IBuildingData data, System.Action<int> action)
    {
        foreach (var button in _upgradeButtons)
        {
            Destroy(button.gameObject);
        }
        _upgradeButtons.Clear();

        if(data.PreviousLevel != null)
        {
            _upgradeButtons.Add(Instantiate(_downgradeButtonRef, _upgradeButtonsHolder));
            _upgradeButtons[_upgradeButtons.Count - 1].onClick.AddListener(() => action(-1));
        }

        for(int i = 0; i < data.Upgrades.Count; i++)
        {
            int j = i;
            _upgradeButtons.Add(Instantiate(_upgradeButtonRef, _upgradeButtonsHolder));
            _upgradeButtons[_upgradeButtons.Count - 1].onClick.AddListener(() => action(j));
        }
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
