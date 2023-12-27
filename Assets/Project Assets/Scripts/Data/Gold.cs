using System;
using UnityEngine;

namespace Project_Assets.Scripts.Data
{
    public class Gold
    {
        private const string GoldKey = "GoldCount"; 
        
        private int _goldCount;
        
        public event Action OnGoldCountChanged; 
        
        public int GoldCount
        {
            get => _goldCount;
            set
            {
                _goldCount = value;
                PlayerPrefs.SetInt(GoldKey, _goldCount);
                OnGoldCountChanged?.Invoke();
            }
        } 
        
        public Gold() =>
            _goldCount = PlayerPrefs.GetInt(GoldKey);
    }
}