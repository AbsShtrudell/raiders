using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
    public class RankManager : MonoBehaviour
    {
        private const string RANK = "rank";
        private const int DEFAULT_VALUE = -1;

        [SerializeField] private int increment = 25;
        [SerializeField] private int defaultRank = 25;

        public event System.Action onRankChanged;

        public void SetRank(int rank)
        {
            PlayerPrefs.SetInt(RANK, rank);
            PlayerPrefs.Save();

            onRankChanged?.Invoke();
        }

        public void ResetRank()
        {
            SetRank(defaultRank);
        }

        public int GetRank()
        {
            int rank = PlayerPrefs.GetInt(RANK, DEFAULT_VALUE);

            if (rank == DEFAULT_VALUE)
                Debug.LogError("Rank pref is not set");

            return rank;
        }

        public void IncrementRank()
        {
            int rank = GetRank();
            
            if (rank != DEFAULT_VALUE)
                SetRank(rank + increment);
        }

        public void DecrementRank()
        {
            int rank = GetRank();
            
            if (rank != DEFAULT_VALUE && rank - increment >= 0)
                SetRank(rank - increment);
        }
    }
}
