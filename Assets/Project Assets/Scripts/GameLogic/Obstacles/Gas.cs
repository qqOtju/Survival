using System;
using System.Collections;
using Project_Assets.Scripts.Data.SO;
using Project_Assets.Scripts.GameLogic.Player;
using UnityEngine;

namespace Project_Assets.Scripts.GameLogic.Obstacles
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D))]
    public class Gas: MonoBehaviour
    {
        [SerializeField] private GasSO _gasSO;
        
        private PlayerController _playerController;
        private Coroutine _timerCoroutine;
        private bool _playerIn;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if(_playerController == null)
                _playerController = other.GetComponent<PlayerController>();
            _timerCoroutine = StartCoroutine(Timer(_gasSO.TimeToDamage, DamagePlayer));
            _playerIn = true;
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            StopCoroutine(_timerCoroutine);
            _playerIn = false;
        }
        
        private IEnumerator Timer(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action();
            if (_playerIn)
                _timerCoroutine = StartCoroutine(Timer(_gasSO.TimeToDamage, DamagePlayer));
        }
        
        private void DamagePlayer() =>
            _playerController.TakeDamage(_gasSO.Damage);
    }
}