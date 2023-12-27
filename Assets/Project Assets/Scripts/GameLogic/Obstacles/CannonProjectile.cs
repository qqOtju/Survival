using Project_Assets.Scripts.Data.SO;
using Project_Assets.Scripts.GameLogic.Player;
using UnityEngine;

namespace Project_Assets.Scripts.GameLogic.Obstacles
{
    [SelectionBase]
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class CannonProjectile: MonoBehaviour
    {
        [SerializeField] private CannonProjectileSO _projectileSO;
        
        private PlayerController _playerController;
        private bool _initialized;
        private Rigidbody2D _rb;
        private float _time;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!_initialized) return;
            _time += Time.deltaTime;
            if (_time >= _projectileSO.LifeTime)
                Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_initialized) return;
            if (!other.CompareTag("Player")) return;
            if(_playerController == null)
                _playerController = other.GetComponent<PlayerController>();
            _playerController.TakeDamage(_projectileSO.Damage);
            Destroy(gameObject);
        }
        
        public void Init(Vector2 direction)
        {
            _rb.velocity = direction * _projectileSO.Speed;
            _initialized = true;
        }
    }
}