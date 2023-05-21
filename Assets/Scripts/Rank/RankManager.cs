using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raiders
{
    public class RankManager : MonoBehaviour
    {
        private const string RANK = "rank";
        private const int DEFAULT_VALUE = -1;

        [SerializeField, Min(0)] private int increment = 25;
        [SerializeField, Min(0)] private int defaultRank = 25;
        [field: SerializeField, Min(0)] public int LeagueCount { get; private set; } = 3;
        [field: SerializeField, Min(0)] public int LeagueCapacity { get; private set; } = 100;

        public event System.Action onRankChanged;

        public void Awake()
        {
            if (!IsRankSet())
                ResetRank();
        }

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

        public bool IsRankSet()
        {
            return PlayerPrefs.GetInt(RANK, DEFAULT_VALUE) != DEFAULT_VALUE;
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
            
            if (rank == DEFAULT_VALUE)
                return;

            SetRank(Mathf.Clamp(rank - increment, 0, int.MaxValue));
        }
    }
}
