using Project_Assets.Scripts.Data.SO;
using Project_Assets.Scripts.GameLogic.Player;
using UnityEngine;

namespace Project_Assets.Scripts.GameLogic.Obstacles
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class Spike: MonoBehaviour
    {
        [SerializeField] private SpikeSO _spikeSO;

        private PlayerController _playerController;
        private Rigidbody2D _rb;
        
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        
        private void FixedUpdate()
        {
            Move();
        }

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
        }
        
        private void Move()
        {
            if(_spikeSO.MoveSpeed == 0 || _playerController == null) return;
            var direction = _playerController.transform.position - transform.position;
            _rb.velocity = direction.normalized * _spikeSO.MoveSpeed;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            _playerController.TakeDamage(_spikeSO.Damage);
        }
    }
}