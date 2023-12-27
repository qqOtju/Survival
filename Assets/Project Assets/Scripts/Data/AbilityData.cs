using System;
using Project_Assets.Scripts.Data.SO;
using UnityEngine;

namespace Project_Assets.Scripts.Data
{
    public class AbilityData
    {
        private const string CountKey = "AbilityCount";
        
        private bool _activated;
        private int _count;

        public event Action<int> OnAbilityUse;
        public event Action<int> OnAbilityEnd; 
        
        public AbilitySO Ability { get; }
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                SetCount(_count);
            }
        }
        public bool Activated
        {
            get => _activated;
            set
            {
                _activated = value;
                if(_activated)
                    OnAbilityUse?.Invoke(Ability.ID);
                else
                    OnAbilityEnd?.Invoke(Ability.ID);
            }
        }
        
        public AbilityData(AbilitySO ability)
        {
            Ability = ability;            
            Activated = false;
            _count = GetCount();
        }
        
        private int GetCount() =>
            PlayerPrefs.GetInt($"{CountKey}{Ability.Name}");
        
        private void SetCount(int count) =>
            PlayerPrefs.SetInt($"{CountKey}{Ability.Name}", count);
    }
}