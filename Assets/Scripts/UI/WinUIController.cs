using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Raiders
{
    public class WinUIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _sideWonText;
        [SerializeField] private TMP_Text _rankText;
        [SerializeField] private Button _backButton;
        [SerializeField] private RankUI _rankUI;

        private void Awake()
        {
            _backButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.Shutdown();

                if (NetworkManager.Singleton != null)
                {
                    Destroy(NetworkManager.Singleton.gameObject);
                }

                SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
            });
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show(Side winner, int rankScore)
        {
            _sideWonText.text = string.Format("{0} Won", winner.ToString());
            _rankText.text = rankScore >= 0? "+25" : "-25" ;
            _rankUI.Refresh();

            gameObject.SetActive(true);
        }
    }
}
