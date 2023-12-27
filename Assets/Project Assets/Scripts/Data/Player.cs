using System;
using Project_Assets.Scripts.Data.SO;

namespace Project_Assets.Scripts.Data
{
    public class Player
    {
        private readonly int _maxHealth;
        
        private int _currentHealth;

        public float AdditionalMoveSpeed;
        
        public event Action<int> OnHealthUpdated;
        public event Action OnPlayerDeath;
        
        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                if(value > _maxHealth)
                    value = _maxHealth;
                _currentHealth = value;
                OnHealthUpdated?.Invoke(value);
                if(value <= 0)
                    OnPlayerDeath?.Invoke();
            }
        }

        public float CurrentMoveSpeed
        {
            get
            {
                var moveSpeed = PlayerSO.MoveSpeed + AdditionalMoveSpeed;
                return moveSpeed;
            }
        }

        public PlayerSO PlayerSO { get; }

        public Player(PlayerSO stats)
        {
            PlayerSO = stats;  
            _maxHealth = PlayerSO.MaxHealth;
            CurrentHealth = PlayerSO.MaxHealth;
        }
        
        public void Reset()
        { 
            AdditionalMoveSpeed = 0;
            CurrentHealth = _maxHealth;
        }
    }
}