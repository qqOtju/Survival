using Project_Assets.Scripts.Audio;
using Project_Assets.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project_Assets.Scripts.UI.MainMenu
{
    public class RecordsUI: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] _records;
        [SerializeField] private GameObject _recordsPanel;
        [SerializeField] private Button _backButton;
        
        private SoundManager _soundManager;
        private GameData _gameData;
        
        [Inject]
        private void Construct(SoundManager soundManager, GameData gameData)
        {
            _soundManager = soundManager;
            _gameData = gameData;
            for (var i = 0; i < _gameData.Score.LastFiveRecords.Length; i++)
            {
                var score = _gameData.Score.LastFiveRecords[i];
                _records[i].text = score.ToString();
            }
        }
        
        private void Awake() =>
            _backButton.onClick.AddListener(OnBackButton);

        private void OnBackButton()
        {
            _soundManager.PlayButtonClick();
            _recordsPanel.SetActive(false);
        }
    }
}