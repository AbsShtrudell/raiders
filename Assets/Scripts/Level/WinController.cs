using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
    public class WinController : MonoBehaviour
    {
        [SerializeField] private LevelController levelController;
        [SerializeField] private RankManager rankManager;

        private void Awake()
        {
            levelController.OnGameEnd += EndGame;
        }

        private void EndGame(Side winSide)
        {
            Debug.Log(winSide + " have won");
        }
    }
}
