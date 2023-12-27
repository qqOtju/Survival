using Project_Assets.Scripts.Data.SO;
using UnityEngine;

namespace Project_Assets.Scripts.Data
{
    public class LevelData
    {
        private const string LevelNumberKey = "LevelNumber";

        private LevelState _levelState;
        
        public LevelSO LevelSO { get; }
        public LevelState LevelState
        {
            get => _levelState;
            set
            {
                _levelState = value;
                PlayerPrefs.SetInt($"{LevelNumberKey}{LevelSO.ID}", (int) value);
            } 
        }

        public LevelData(LevelSO level)
        {
            LevelSO = level;
            LevelState = (LevelState) PlayerPrefs.
                GetInt($"{LevelNumberKey}{level.ID}", 0);
            if (level.ID == 0 && LevelState != LevelState.Completed)
                LevelState = LevelState.Unlocked;
        }
    }
    public enum LevelState
    {
        Locked = 0,
        Unlocked = 1,
        Completed = 2
    }
}