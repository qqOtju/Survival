using UnityEngine;

namespace Project_Assets.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "Spike", menuName = "MyAssets/Spike")]
    public class SpikeSO: ScriptableObject
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private int _damage; 
        
        public float MoveSpeed => _moveSpeed;
        public int Damage => _damage;
    }
}