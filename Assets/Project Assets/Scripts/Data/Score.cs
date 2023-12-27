using System;
using System.Linq;
using UnityEngine;

namespace Project_Assets.Scripts.Data
{
    public class Score
    {
        private const string HighScoreKey = "Highscore";
        
        private int _currentScore;
        private int _currentGold;

        public bool DoubleScore;
        public int[] LastFiveRecords { get; }
        public int CurrentScore
        {
            get => _currentScore;
            set
            {
                _currentScore = value;
                OnScoreUpdated?.Invoke(value);
            }
        }
        
        public int CurrentGold
        {
            get => _currentGold;
            set
            {
                _currentGold = value;
                OnGoldUpdated?.Invoke(value);
            }
        }
        
        public event Action<int> OnScoreUpdated;
        public event Action<int> OnGoldUpdated; 

        public Score()
        {
            LastFiveRecords = new int[5];
            for (var i = 0; i < LastFiveRecords.Length; i++)
                LastFiveRecords[i] = PlayerPrefs.GetInt($"{HighScoreKey}{i}", 0);
            Array.Sort(LastFiveRecords);
        }
        
        public void UpdateHighScore(int newScore)
        {
            var isBetter = LastFiveRecords.Any(scr => newScore > scr);
            if (isBetter)
            {
                LastFiveRecords[0] = newScore;
                Array.Sort(LastFiveRecords);
            }
            for (var i = 0; i < LastFiveRecords.Length; i++)
                PlayerPrefs.SetInt($"{HighScoreKey}{i}", LastFiveRecords[i]);
        }
    }
}