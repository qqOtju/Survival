using UnityEngine;

namespace Project_Assets.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "PlayerSO", menuName = "MyAssets/PlayerSO")]
    public class PlayerSO: ScriptableObject
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _moveSpeed;
        
        public int MaxHealth => _maxHealth;
        public float MoveSpeed => _moveSpeed;
    }
}