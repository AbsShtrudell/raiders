using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Raiders
{
    public class RankUI : MonoBehaviour
    {
        [SerializeField] private int maxRank = 100;
        [SerializeField] private Slider slider;
        [SerializeField] private Image image;
        [SerializeField] private RankManager rankManager;
        [SerializeField] private Sprite[] rankImages;

        private void Awake()
        {
            slider.maxValue = maxRank;
            rankManager.onRankChanged += Refresh;
            Refresh();
        }

        public void Refresh()
        {
            int overallRank = rankManager.GetRank();

            int rank = overallRank % maxRank;
            int league = overallRank / maxRank;

            slider.value = rank;
            image.sprite = rankImages[league];
        }
    }
}
