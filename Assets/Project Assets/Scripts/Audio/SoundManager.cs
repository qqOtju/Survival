using UnityEngine;

namespace Project_Assets.Scripts.Audio
{
    public class SoundManager: MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource _audioSourceMusic;
        [SerializeField] private AudioSource _audioSourceSound;
        [Header("Audio Clips")]
        [SerializeField] private AudioClip _buttonClickClip;
        [SerializeField] private AudioClip _music;

        private void Awake() =>
            DontDestroyOnLoad(gameObject);

        private void Start()
        {
            _audioSourceMusic.clip = _music;
            _audioSourceMusic.loop = true;
            _audioSourceMusic.Play();
        }

        public void MusicVolume(float value) =>
            _audioSourceMusic.volume = value;

        public void SoundVolume(float value) =>
            _audioSourceSound.volume = value;

        public void PlayButtonClick() =>
            _audioSourceSound.PlayOneShot(_buttonClickClip);

        public void Sound(AudioClip clip) =>
            _audioSourceSound.PlayOneShot(clip);
        
        public void Music(AudioClip music)
        { 
            _audioSourceMusic.Stop();
            _audioSourceMusic.clip = music;
            _audioSourceMusic.loop = true;
            _audioSourceMusic.Play();
        }
    }
}