using Project_Assets.Scripts.Audio;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project_Assets.Scripts.UI.MainMenu
{
    public class SettingsUI: MonoBehaviour
    {
        [Header("Sliders")]
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _soundSlider;
        [Header("Panels")]
        [SerializeField] private GameObject _settingsPanel;
        [Header("Buttons")]
        [SerializeField] private Button _backButton;
        
        private const string MusicVolumeKey = "MusicVolume";
        private const string SoundVolumeKey = "SoundVolume";
        
        private SoundManager _soundManager;
        
        [Inject]
        private void Construct(SoundManager soundManager)
        {
            _soundManager = soundManager;
            var musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
            var effectsVolume = PlayerPrefs.GetFloat(SoundVolumeKey, 0.5f);
            Music(musicVolume);
            Sound(effectsVolume);
        }
        
        private void Awake()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
            _musicSlider.onValueChanged.AddListener(OnMusicVolumeChange);
            _soundSlider.onValueChanged.AddListener(OnSoundVolumeChange);
        }

        private void OnDestroy()
        {
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
            _musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChange);
            _soundSlider.onValueChanged.RemoveListener(OnSoundVolumeChange);
        }

        private void OnBackButtonClicked()
        {
            _soundManager.PlayButtonClick();
            _settingsPanel.SetActive(false);
        }
        
        private void OnMusicVolumeChange(float value) =>
            Music(value);

        private void OnSoundVolumeChange(float value) =>
            Sound(value);

        private void Music(float value)
        {
            _soundManager.MusicVolume(value);
            _musicSlider.value = value;
            PlayerPrefs.SetFloat(MusicVolumeKey, value);
        }

        private void Sound(float value)
        {
            _soundManager.SoundVolume(value);
            _soundSlider.value = value;
            PlayerPrefs.SetFloat(SoundVolumeKey, value);
        }
    }
}