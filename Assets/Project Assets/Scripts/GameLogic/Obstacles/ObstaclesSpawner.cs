using System.Collections;
using System.Collections.Generic;
using Project_Assets.Scripts.Audio;
using Project_Assets.Scripts.Data;
using Project_Assets.Scripts.Data.SO;
using Project_Assets.Scripts.GameLogic.Gold;
using Project_Assets.Scripts.GameLogic.Item;
using Project_Assets.Scripts.GameLogic.Player;
using Project_Assets.Scripts.UI.GameScene;
using UnityEngine;
using UnityEngine.UI;
using VInspector;
using Zenject;
using Random = UnityEngine.Random;

namespace Project_Assets.Scripts.GameLogic.Obstacles
{
    public class ObstaclesSpawner : MonoBehaviour
    {
        [Header("Header")] 
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Transform _playerShopPosition;
        [SerializeField] private GameObject _tapToStartUI;
        [SerializeField] private Button _tapToStartButton;
        [SerializeField] private ResultsUI _resultsUI;
        [Tab("Cannon")] 
        [SerializeField] private Cannon _cannonPrefab;
        [SerializeField] private Transform _cannonContainer;
        [SerializeField] private Transform[] _cannonSpawnPoints;
        [SerializeField] private Vector2 _minMaxCannonsCount;
        [Tab("Static spike")]
        [SerializeField] private Spike _staticSpikePrefab;
        [SerializeField] private Transform _staticSpikeContainer;
        [SerializeField] private Transform[] _staticSpikeSpawnPoints;
        [SerializeField] private Vector2 _minMaxStaticSpikesCount;
        [Tab("Moving spike")]
        [SerializeField] private Spike _movingSpikePrefab;
        [SerializeField] private Transform _movingSpikeContainer;
        [SerializeField] private Transform[] _movingSpikeSpawnPoints;
        [SerializeField] private Vector2 _minMaxMovingSpikesCount;
        [Tab("Gas")]
        [SerializeField] private Gas _gasPrefab;
        [SerializeField] private Transform _gasContainer;
        [SerializeField] private Transform[] _gasSpawnPoints;
        [SerializeField] private Vector2 _minMaxGasCount;
        [Tab("Gold")]
        [SerializeField] private GoldController _goldPrefab;
        [SerializeField] private Transform _goldContainer;
        [SerializeField] private Transform[] _goldSpawnPoints;
        [Tab("Items")]
        [SerializeField] private ItemSO[] _itemsSO;
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private Transform[] _itemsSpawnPoints;

        private readonly List<GoldController> _goldControllers = new();
        private readonly List<ItemController> _items = new();
        private readonly List<Spike> _staticSpikes = new();
        private readonly List<Spike> _movingSpikes = new();
        private readonly List<Cannon> _cannons = new();
        private readonly List<Gas> _gases = new();

        private SoundManager _soundManager;
        private GameData _gameData; 
        private int _itemsCollected;
        private bool _playerDead = false;
        
        private const int MaxItemsCount = 3;
        private const float ShopDuration = 15f;

        [Inject]
        private void Construct(GameData gameData, SoundManager soundManager)
        {
            _gameData = gameData;
            _soundManager = soundManager;
            _gameData.Score.CurrentScore = 0;
            _gameData.Score.CurrentGold = 0;
        }
        
        private void Awake()
        {
            _tapToStartButton.onClick.AddListener(OnTapToStartButtonClicked);
            _playerController.OnPlayerDeath += OnPlayerDeath;
        }

        private void OnTapToStartButtonClicked()
        {
            _tapToStartUI.SetActive(false);
            StartWave();
        }

        private void OnPlayerDeath()
        {
            if(_playerDead) return;
            _playerDead = true;
            _gameData.Score.UpdateHighScore(_gameData.Score.CurrentScore);
            _gameData.Gold.GoldCount += _gameData.Score.CurrentScore;
            _resultsUI.gameObject.SetActive(true);
        }

        private void StartWave()
        {
            SpawnCannons();
            SpawnStaticSpikes();
            SpawnMovingSpikes();
            SpawnGas();
            SpawnGold();
            StartCoroutine(StartWaveTimer());
        }

        private IEnumerator StartWaveTimer()
        {
            yield return new WaitForSeconds(_gameData.Wave.WaveDuration);
            StopWave();
            StartShop();
        }

        private void StopWave()
        {
            foreach (var gold in _goldControllers)
            {
                gold.OnGoldCollected -= OnGoldCollected;
                Destroy(gold.gameObject);
            }
            _goldControllers.Clear();
            foreach (var spike in _staticSpikes)
                Destroy(spike.gameObject);
            _staticSpikes.Clear();
            foreach (var spike in _movingSpikes)
                Destroy(spike.gameObject);
            _movingSpikes.Clear();
            foreach (var cannon in _cannons)
                Destroy(cannon.gameObject);
            _cannons.Clear();
            foreach (var gas in _gases)
                Destroy(gas.gameObject);
            _gases.Clear();
        }

        private void StartShop()
        {
            _gameData.Wave.Reset();
            _gameData.Score.DoubleScore = false;
            _playerController.transform.position = _playerShopPosition.position;
            SpawnItems();
            StartCoroutine(StartShopTimer());
        }

        private void SpawnItems()
        {
            _itemsCollected = 0;
            if(_itemsSpawnPoints.Length < MaxItemsCount) 
                throw new UnityException("Items spawn points count must be more than max items count");
            if(_itemsSO.Length < MaxItemsCount) 
                throw new UnityException("Items SO count must be more than max items count");
            var usedSpawnPoints = new List<Transform>();
            var usedItems = new List<ItemSO>();
            for (var i = 0; i < MaxItemsCount; i++)
            {
                var spawnPoint = _itemsSpawnPoints[Random.Range(0, _itemsSpawnPoints.Length)];
                while (usedSpawnPoints.Contains(spawnPoint))
                    spawnPoint = _itemsSpawnPoints[Random.Range(0, _itemsSpawnPoints.Length)];
                usedSpawnPoints.Add(spawnPoint);
                var itemSO = _itemsSO[Random.Range(0, _itemsSO.Length)];
                while (usedItems.Contains(itemSO))
                    itemSO = _itemsSO[Random.Range(0, _itemsSO.Length)];
                usedItems.Add(itemSO);
                var item = Instantiate(itemSO.ItemPrefab, spawnPoint.position, spawnPoint.rotation, _itemsContainer);
                item.Init(_gameData, _soundManager);
                item.OnItemCollected += OnItemCollected;
                _items.Add(item);
            }
        }

        private void OnItemCollected(ItemController obj)
        {
            _itemsCollected++;
            if (_itemsCollected != MaxItemsCount) return;
            StopAllCoroutines();
            StopShop();
            StartWave();
        }

        private IEnumerator StartShopTimer()
        {
            yield return new WaitForSeconds(ShopDuration);
            StopShop();
            StartWave();
        }

        private void StopShop()
        {
            foreach (var item in _items)
                if(item != null)
                    Destroy(item.gameObject);
            _items.Clear();
        }

        private void SpawnGold()
        {
            var spawnPoint = _goldSpawnPoints[Random.Range(0, _goldSpawnPoints.Length)];
            var gold = Instantiate(_goldPrefab, spawnPoint.position, spawnPoint.rotation, _goldContainer);
            _goldControllers.Add(gold);
            gold.Init(_soundManager);
            gold.OnGoldCollected += OnGoldCollected;
        }

        private void SpawnGas()
        {
            var gasCount = Random.Range((int)_minMaxGasCount.x, (int)_minMaxGasCount.y);
            if(gasCount == 0) return;
            var usedSpawnPoints = new List<Transform>();
            for (var i = 0; i < gasCount; i++)
            {
                var spawnPoint = _gasSpawnPoints[Random.Range(0, _gasSpawnPoints.Length)];
                while (usedSpawnPoints.Contains(spawnPoint))
                    spawnPoint = _gasSpawnPoints[Random.Range(0, _gasSpawnPoints.Length)];
                usedSpawnPoints.Add(spawnPoint);
                var gas = Instantiate(_gasPrefab, spawnPoint.position, spawnPoint.rotation, _gasContainer);
                _gases.Add(gas);
            }
        }

        private void SpawnMovingSpikes()
        {
            var movingSpikesCount = Random.Range((int)_minMaxMovingSpikesCount.x, (int)_minMaxMovingSpikesCount.y);
            if(movingSpikesCount == 0) return;
            var usedSpawnPoints = new List<Transform>();
            for (var i = 0; i < movingSpikesCount; i++)
            {
                var spawnPoint = _movingSpikeSpawnPoints[Random.Range(0, _movingSpikeSpawnPoints.Length)];
                while (usedSpawnPoints.Contains(spawnPoint))
                    spawnPoint = _movingSpikeSpawnPoints[Random.Range(0, _movingSpikeSpawnPoints.Length)];
                usedSpawnPoints.Add(spawnPoint);
                var movingSpike = Instantiate(_movingSpikePrefab, spawnPoint.position, spawnPoint.rotation, _movingSpikeContainer);
                movingSpike.Init(_playerController);
                _movingSpikes.Add(movingSpike);
            }
        }

        private void SpawnStaticSpikes()
        {
            var staticSpikesCount = Random.Range((int)_minMaxStaticSpikesCount.x, (int)_minMaxStaticSpikesCount.y);
            if(staticSpikesCount == 0) return;
            var usedSpawnPoints = new List<Transform>();
            for (var i = 0; i < staticSpikesCount; i++)
            {
                var spawnPoint = _staticSpikeSpawnPoints[Random.Range(0, _staticSpikeSpawnPoints.Length)];
                while (usedSpawnPoints.Contains(spawnPoint))
                    spawnPoint = _staticSpikeSpawnPoints[Random.Range(0, _staticSpikeSpawnPoints.Length)];
                usedSpawnPoints.Add(spawnPoint);
                var staticSpike = Instantiate(_staticSpikePrefab, spawnPoint.position, spawnPoint.rotation, _staticSpikeContainer);
                staticSpike.Init(_playerController);
                _staticSpikes.Add(staticSpike);
            }
        }

        private void SpawnCannons()
        {
            var cannonsCount = Random.Range((int)_minMaxCannonsCount.x, (int)_minMaxCannonsCount.y);
            if(cannonsCount == 0) return;
            var usedSpawnPoint = new List<Transform>();
            for (var i = 0; i < cannonsCount; i++)
            {
                var spawnPoint = _cannonSpawnPoints[Random.Range(0, _cannonSpawnPoints.Length)];
                while (usedSpawnPoint.Contains(spawnPoint))
                    spawnPoint = _cannonSpawnPoints[Random.Range(0, _cannonSpawnPoints.Length)];
                usedSpawnPoint.Add(spawnPoint);
                var cannon = Instantiate(_cannonPrefab, spawnPoint.position, spawnPoint.rotation, _cannonContainer);
                cannon.Init(_playerController, _soundManager);
                _cannons.Add(cannon);
            }
        }
        
        private void OnGoldCollected(GoldController gold)
        {
            gold.OnGoldCollected -= OnGoldCollected;
            _goldControllers.Remove(gold);
            _gameData.Score.CurrentScore += _gameData.Score.DoubleScore ? 2 : 1;
            _gameData.Score.CurrentGold++;
            SpawnGold();
        }
    }
}