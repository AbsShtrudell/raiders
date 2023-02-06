using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Raiders.UI
{
    public class CoinsController : MonoBehaviour
    {
        [SerializeField]
        private LevelController levelController;
        [SerializeField]
        private PlayerController playerController;
        [SerializeField]
        private TextMeshProUGUI coinsAmountText;

        private void Start()
        {
            SideController sideController;
            levelController.SideControllers.TryGetValue(playerController.Side, out sideController);

            if (sideController != null)
            {
                sideController.CoinsChanged += ChangeCoinsAmountText;
                ChangeCoinsAmountText(sideController.Coins);
            }
        }

        private void ChangeCoinsAmountText(int coinsAmount)
        {
            coinsAmountText.text = coinsAmount.ToString();
        }
    }
}