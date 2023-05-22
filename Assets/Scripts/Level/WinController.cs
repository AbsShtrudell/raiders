using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Raiders
{
    public class WinController : MonoBehaviour
    {
        [SerializeField] private LevelController levelController;
        [SerializeField] private RankManager rankManager;
        [SerializeField] private WinUIController winUIController;

        private void Awake()
        {
            levelController.OnGameEnd += EndGame;
        }

        private void EndGame(Side winSide)
        {
            Debug.Log(winSide + " have won");

            if(winSide == (NetworkManager.Singleton.IsHost? Side.Vikings : Side.English))
            {
                rankManager.IncrementRank();
                winUIController.Show(winSide, rankManager.Increment);
            }
            else
            {
                rankManager.DecrementRank();
                winUIController.Show(winSide, -rankManager.Increment);
            }
        }
    }
}
