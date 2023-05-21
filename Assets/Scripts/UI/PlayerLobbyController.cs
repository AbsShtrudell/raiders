using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Raiders
{
    public class PlayerLobbyController : MonoBehaviour
    {
        [SerializeField] private Image readyIcon;
        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private Sprite readySprite;
        [SerializeField] private Sprite unReadySprite;

        private void Awake()
        {
            IsReady(false);
        }

        public void SetPlayerName(string name)
        {
            playerName.text = name;
        }

        public void IsReady(bool ready)
        {
            readyIcon.sprite = ready? readySprite : unReadySprite;
        }
    }
}
