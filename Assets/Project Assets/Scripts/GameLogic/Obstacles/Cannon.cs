using Project_Assets.Scripts.Audio;
using Project_Assets.Scripts.Data.SO;
using Project_Assets.Scripts.GameLogic.Player;
using UnityEngine;

namespace Project_Assets.Scripts.GameLogic.Obstacles
{
    [SelectionBase]
    public class Cannon: MonoBehaviour
    {
        [SerializeField] private CannonSO _cannonSO;
        [SerializeField] private ParticleSystem _shootEffect;
        [SerializeField] private AudioClip _shootClip;
        
        private PlayerController _playerController;
        private SoundManager _soundManager;
        private Transform _transform;
        private float _attackRate;
        private float _time;
        
        private void Start()
        {
            _transform = transform;
            _attackRate = Random.Range(_cannonSO.AttackRate.x, _cannonSO.AttackRate.y);
        }
        
        private void FixedUpdate()
        {
            Rotate();
            Shoot();
        }
        
        public void Init(PlayerController playerController, SoundManager soundManager)
        {
            _playerController = playerController;
            _soundManager = soundManager;
        }

        private void Shoot()
        {
            _time += Time.deltaTime;
            if (!(_time >= _attackRate)) return;
            _soundManager.Sound(_shootClip);
            _shootEffect.Play();
            _time = 0;
            var projectile = Instantiate(_cannonSO.ProjectilePrefab, _transform.position, _transform.rotation);
            projectile.Init(_transform.right);
        }

        private void Rotate()
        {
            if(_playerController == null) return;
            var direction = _playerController.transform.position - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _cannonSO.RotationSpeed * Time.deltaTime);
        }
    }
}