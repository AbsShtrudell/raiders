using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Raiders
{
    public class RankUI : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Image image;
        [SerializeField] private RankManager rankManager;
        [SerializeField] private Sprite placeholderImage;
        [SerializeField] private Sprite[] rankImages;

        private void Awake()
        {
            slider.maxValue = rankManager.LeagueCapacity;
            rankManager.onRankChanged += Refresh;
            Refresh();
        }

        public void Refresh()
        {
            int overallRank = rankManager.GetRank();

            int rank = overallRank % rankManager.LeagueCapacity;
            int league = overallRank / rankManager.LeagueCapacity;

            slider.value = rank;

            if (league < rankImages.Length)
                image.sprite = rankImages[league];
            else
                image.sprite = placeholderImage;
        }
    }
}
