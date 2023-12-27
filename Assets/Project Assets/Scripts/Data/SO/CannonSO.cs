using Project_Assets.Scripts.GameLogic.Obstacles;
using UnityEngine;

namespace Project_Assets.Scripts.Data.SO
{
    [CreateAssetMenu(fileName = "Cannon", menuName = "MyAssets/Cannon")]
    public class CannonSO: ScriptableObject
    {
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private Vector2 _attackRate;
        [SerializeField] private CannonProjectile _projectilePrefab;
        
        public float RotationSpeed => _rotationSpeed;
        public Vector2 AttackRate => _attackRate;
        public CannonProjectile ProjectilePrefab => _projectilePrefab;
    }
}