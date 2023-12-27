using System;
using LeanTween.Framework;
using Project_Assets.Scripts.Audio;
using Project_Assets.Scripts.Data;
using Project_Assets.Scripts.InputControl;
using UnityEngine;
using Zenject;

namespace Project_Assets.Scripts.GameLogic.Player
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class PlayerController: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private Transform _view;
        [SerializeField] private AudioClip _clip;

        private SoundManager _soundManager;
        private InputHandlerController _inputHandlerController;
        private Vector3 _baseScale;
        private GameData _gameData;
        private Rigidbody2D _rb;
        
        private float MoveSpeed => _gameData.Player.PlayerSO.MoveSpeed;
        
        public event Action OnPlayerDeath; 
        
        [Inject]
        private void Construct(InputHandlerController inputHandlerController, GameData gameData, SoundManager soundManager)
        {
            _gameData = gameData;
            _soundManager = soundManager;
            _gameData.Player.Reset();
            _inputHandlerController = inputHandlerController;
            inputHandlerController.OnMove += OnMove;
        }

        private void Start()
        {
            _baseScale = _view.transform.localScale;
            _rb = GetComponent<Rigidbody2D>();
        }
        
        public void TakeDamage(int damage)
        {
            _gameData.Player.CurrentHealth -= damage;
            _soundManager.Sound(_clip);
            LeanTween.Framework.LeanTween.scale(_view.gameObject, _baseScale * 1.2f, .2f)
                .setEase(LeanTweenType.easeInOutSine).setOnComplete(()=>_view.transform.localScale = _baseScale);
            LeanTween.Framework.LeanTween.color(_sr.gameObject, Color.red, .2f)
                .setEase(LeanTweenType.easeInOutSine).setOnComplete(()=>_sr.color = Color.white);
            if (_gameData.Player.CurrentHealth <= 0)
                Die();
        }

        private void OnMove(Vector2 obj)
        {
            _rb.velocity = obj * _gameData.Player.CurrentMoveSpeed;             
        }

        private void Die()
        {
            _inputHandlerController.OnMove -= OnMove;
            _rb.velocity = Vector2.zero;
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            LeanTween.Framework.LeanTween.cancel(_view.gameObject);
            LeanTween.Framework.LeanTween.cancel(_sr.gameObject);
            OnPlayerDeath?.Invoke();
        }
    }
}