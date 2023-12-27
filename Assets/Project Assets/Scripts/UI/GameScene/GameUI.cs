using System.Collections;
using System.Collections.Generic;
using LeanTween.Framework;
using Project_Assets.Scripts.Data;
using TMPro;
using Tools.MyGridLayout;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VInspector;
using Zenject;

namespace Project_Assets.Scripts.UI.GameScene
{
    public class GameUI: MonoBehaviour
    {
        [Tab("Health")]
        [Header("References")]
        [SerializeField] private HealthLayout _healthLayout;
        [Header("Health Settings")]
        [SerializeField] private Image _healthSprite;
        [SerializeField] private Color _healthColor;
        [SerializeField] private Color _healthLostColor;
        [Tab("Abilities")]
        [Header("Abilities")] 
        [SerializeField] private TextMeshProUGUI[] _abilitiesNames;
        [SerializeField] private TextMeshProUGUI[] _abilitiesCountText;
        [SerializeField] private Button[] _abilitiesButtons;
        [Header("Abilities Images")]
        [SerializeField] private Image[] _abilitiesImages;
        [SerializeField] private Image[] _abilitiesFillImages;
        [SerializeField] private Image[] _abilitiesButtonImages;
        [Tab("Other")]
        [Header("Buttons")]
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _restartButton;
        [Header("Other")]
        [SerializeField] private string _mainMenuSceneName;
        [SerializeField] private string _gameSceneName;
        [SerializeField] private TextMeshProUGUI _counterText;
        [SerializeField] private TextMeshProUGUI _goldCountText;

        private readonly List<Image> _healthImages = new();
        
        private GameData _gameData;
        private bool _paused;
        
        private int MaxHealth => _gameData.Player.PlayerSO.MaxHealth;

        [Inject]
        private void Construct(GameData dataOfGame)
        {
            _gameData = dataOfGame;
            if (_healthLayout != null)
            {
                _gameData.Player.OnHealthUpdated += OnPlayerHit;
                for (var i = 0; i < MaxHealth; i++)
                {
                    var image = Instantiate(_healthSprite, _healthLayout.transform);
                    image.color = _healthColor;
                    _healthImages.Add(image);
                }
                _healthLayout.Align(MaxHealth);
            }
            if(_counterText != null)
            {
                _gameData.Score.OnScoreUpdated += OnScore;
                _counterText.text = "0";
            }
            if(_goldCountText != null)
            {
                _gameData.Score.OnGoldUpdated += OnGold;
                _goldCountText.text = "0";
            }
            if(_abilitiesButtons.Length != _gameData.Abilities.Length)
                throw new System.Exception("Abilities buttons count must be equal to abilities count");
            if(_gameData.Abilities.Length > 0 && _abilitiesNames != null)
                foreach (var ability in _gameData.Abilities)
                {
                    var id = ability.Ability.ID;
                    _abilitiesNames[id].text = ability.Ability.Name;
                    _abilitiesCountText[id].text = "x" + ability.Count;
                    _abilitiesButtons[id].interactable = ability.Count > 0;
                    _abilitiesImages[id].sprite = ability.Ability.Icon;
                    _abilitiesFillImages[id].sprite = ability.Ability.Icon;
                    _abilitiesButtonImages[id].sprite = ability.Ability.Icon;
                    _abilitiesButtons[id].onClick.AddListener(() => OnAbilityUse(ability));
                }
        }
        
        private void Awake()
        {
            _homeButton.onClick.AddListener(OnHomeButtonClicked);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        private void OnDestroy()
        {
            _gameData.Player.OnHealthUpdated -= OnPlayerHit;
            _gameData.Score.OnScoreUpdated -= OnScore;
            _homeButton.onClick.RemoveListener(OnHomeButtonClicked);
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        }
        
        private void OnScore(int score)
        {
            _counterText.text = score.ToString();
            LeanTween.Framework.LeanTween.scale(_counterText.gameObject, Vector3.one * 1.2f, 0.1f)
                .setEase(LeanTweenType.easeOutBack)
                .setOnComplete(() =>
                {
                    LeanTween.Framework.LeanTween.scale(_counterText.gameObject, Vector3.one, 0.1f)
                        .setEase(LeanTweenType.easeInBack);
                });
        }
        
        
        private void OnGold(int obj)
        {
            _goldCountText.text = obj.ToString();
        }

        private void OnRestartButtonClicked() =>
            SceneManager.LoadScene(_gameSceneName, LoadSceneMode.Single);

        private void OnHomeButtonClicked() =>
            SceneManager.LoadScene(_mainMenuSceneName, LoadSceneMode.Single);
        
        private void OnPlayerHit(int value)
        {
            if(value < 0) return;
            for (int i = 0; i < value; i++)
                _healthImages[i].color = _healthColor;
            for (int i = value; i < MaxHealth; i++)
                _healthImages[i].color = _healthLostColor;
        }

        private void OnAbilityUse(AbilityData dataAbility)
        {
            if(dataAbility.Count <= 0) return;
            var id = dataAbility.Ability.ID;
            StartCoroutine(AbilityCooldown(_abilitiesFillImages[id], 
                _abilitiesButtons[id], dataAbility.Ability.Cooldown, dataAbility));
            _abilitiesCountText[id].text = "x" + --dataAbility.Count;
            dataAbility.Activated = true;
        }

        private IEnumerator AbilityCooldown(Image fill,Button button, float duration, AbilityData dataAbility)
        {
            var time = 0f;
            button.interactable = false;
            while (time < duration)
            {
                fill.fillAmount = time / duration;
                time += Time.deltaTime;
                yield return null;
            }
            button.interactable = dataAbility.Count > 0;
        }
    }
}