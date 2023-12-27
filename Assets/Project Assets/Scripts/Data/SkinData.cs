using Project_Assets.Scripts.Data.SO;
using UnityEngine;

namespace Project_Assets.Scripts.Data
{
    public class SkinData
    {
        private const string SkinKey = "Skin";

        private SkinState _skinState;
        
        public SkinSO Skin { get; }
        public SkinState SkinState
        {
            get => _skinState;
            set
            {
                _skinState = value;
                SetSkinState();
            } 
        }
        
        public SkinData(SkinSO skin)
        {
            Skin = skin;
            SkinState = (SkinState) PlayerPrefs.
                GetInt($"{SkinKey}{skin.ID}", 0);
            if (skin.ID == 0 && SkinState != SkinState.Equipped)
                SkinState = SkinState.Unlocked;
        }
        
        private void SetSkinState() =>
            PlayerPrefs.SetInt($"{SkinKey}{Skin.ID}", (int) SkinState);
    }
    
    public enum SkinState
    {
        Locked = 0,
        Unlocked = 1,
        Equipped = 2
    }
}