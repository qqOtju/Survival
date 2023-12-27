using Project_Assets.Scripts.Audio;
using Project_Assets.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VInspector;
using Zenject;

namespace Project_Assets.Scripts.UI.MainMenu
{
    public class MenuUI: MonoBehaviour
    {
        [Tab("Buttons")]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _recordsButton;
        [Tab("Panels")]
        [SerializeField] private GameObject _shopPanel;
        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private GameObject _recordsPanel;
        [Tab("Values")]
        [SerializeField] private TextMeshProUGUI _recordText;
        [SerializeField] private TextMeshProUGUI _goldCountText;
        [Tab("Other")]
        [SerializeField] private string _gameSceneName;

        private SoundManager _soundManager;
        private GameData _data;
        
        [Inject]
        private void Construct(GameData data, SoundManager soundManager)
        { 
            _soundManager = soundManager;
            _data = data;
            SetBestScore();
            SetGoldCount();
            _data.Gold.OnGoldCountChanged += SetGoldCount;
        }
        
        private void Awake()
        {
            _playButton.onClick.AddListener(OnStartButton);
            _shopButton.onClick.AddListener(OnShopButton);
            _settingsButton.onClick.AddListener(OnOptionsButton);
            _recordsButton.onClick.AddListener(OnHistoryButton);
        }
        
        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(OnStartButton);
            _shopButton.onClick.RemoveListener(OnShopButton);
            _settingsButton.onClick.RemoveListener(OnOptionsButton);
            _recordsButton.onClick.RemoveListener(OnHistoryButton);
            _data.Gold.OnGoldCountChanged -= SetGoldCount;
        }

        private void OnStartButton()
        { 
            _soundManager.PlayButtonClick();
            SceneManager.LoadScene(_gameSceneName, LoadSceneMode.Single);
        }

        private void OnOptionsButton()
        {
            _soundManager.PlayButtonClick();
            _settingsPanel.SetActive(true);
        }

        private void OnShopButton()
        {
            _soundManager.PlayButtonClick();
            _shopPanel.SetActive(true);
        }

        private void OnHistoryButton()
        {
            _soundManager.PlayButtonClick();
            _recordsPanel.SetActive(true);
        }
        
        private void SetGoldCount()
        { 
            if(_goldCountText != null)
                _goldCountText.text = _data.Gold.GoldCount.ToString();
        }
        
        private void SetBestScore()
        {
            if(_recordText != null)
                _recordText.text = _data.Score.LastFiveRecords[^1].ToString();
        }
    }
}