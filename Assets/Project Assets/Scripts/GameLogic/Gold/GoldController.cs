using System;
using Project_Assets.Scripts.Audio;
using UnityEngine;

namespace Project_Assets.Scripts.GameLogic.Gold
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D))]
    public class GoldController: MonoBehaviour
    {
        [SerializeField] private ParticleSystem _ps;
        [SerializeField] private AudioClip _clip;

        private SoundManager _soundManager;
        
        public event Action<GoldController> OnGoldCollected;

        public void Init(SoundManager soundManager)
        {
            _soundManager = soundManager;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _soundManager.Sound(_clip);
                _ps.transform.SetParent(transform.parent);
                _ps.Play();
                OnGoldCollected?.Invoke(this);
                Destroy(gameObject);
            }
        }
    }
}