using System;
using Project_Assets.Scripts.Audio;
using Project_Assets.Scripts.Data;
using Project_Assets.Scripts.Data.SO;
using TMPro;
using UnityEngine;

namespace Project_Assets.Scripts.GameLogic.Item
{
    [SelectionBase]
    [RequireComponent(typeof(Collider2D))]
    public abstract class ItemController: MonoBehaviour
    {
        [SerializeField] private ItemSO _itemSO;
        [SerializeField] private TextMesh _priceText;
        [SerializeField] private TextMesh _nameText;
        [SerializeField] private AudioClip _clip;

        private SoundManager _soundManager;
        protected GameData GameData;
        
        public event Action<ItemController> OnItemCollected;
        
        public void Init(GameData gameData, SoundManager soundManager)
        {
            GameData = gameData;
            _soundManager = soundManager;
        }
        
        private void Start()
        {
            _priceText.text = _itemSO.Price.ToString();
            _nameText.text = _itemSO.Name;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.CompareTag("Player")) return;
            if(_itemSO.Price > GameData.Score.CurrentGold) return;
            _soundManager.Sound(_clip);
            GameData.Score.CurrentGold -= _itemSO.Price;
            OnPlayerCollision();
            OnItemCollected?.Invoke(this);
            Destroy(gameObject);
        }
        
        protected abstract void OnPlayerCollision();
    }
}