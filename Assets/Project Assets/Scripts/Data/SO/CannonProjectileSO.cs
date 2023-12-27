using UnityEngine;

namespace Project_Assets.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "CannonProjectile", menuName = "MyAssets/CannonProjectile")]
    public class CannonProjectileSO: ScriptableObject
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _speed;
        [SerializeField] private float _lifeTime;
        
        public int Damage => _damage;
        public float Speed => _speed;
        public float LifeTime => _lifeTime;
    }
}