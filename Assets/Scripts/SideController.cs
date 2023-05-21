using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Raiders
{
    public class SideController
    {
        private int _coins;

        public int Coins
        { get { return _coins; } set { _coins = value; CoinsChanged?.Invoke(Coins); } }

        public event Action<int> CoinsChanged;

        public SideController()
        {
            _coins = 0;
        }

        public bool CanSpendCoins(uint coinsAmount)
        {
            return _coins >= coinsAmount;
        }

        public void SpendCoins(uint coinsAmount)
        {
            _coins = Mathf.Clamp(_coins - (int)coinsAmount, 0, _coins);
            CoinsChanged?.Invoke(Coins);
        }

        public void AddCoins(uint coinsAmount)
        {
            _coins += (int)coinsAmount;
            CoinsChanged?.Invoke(Coins);
        }

    }
}