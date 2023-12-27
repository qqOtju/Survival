using Project_Assets.Scripts.Audio;
using Project_Assets.Scripts.Data;
using Project_Assets.Scripts.Data.SO;
using Project_Assets.Scripts.InputControl;
using UnityEngine;
using Zenject;

namespace Project_Assets.Scripts.Infrastructure.Project
{
    public class ProjectInstaller: MonoInstaller
    {
        [SerializeField] private GameDataSO _gameDataSO;
        [SerializeField] private SoundManager _soundManager;

        public override void InstallBindings()
        {
            BindGame();
            BindSound();
            BindInput();
        }

        private void BindGame()
        {
            var gameData = new GameData(_gameDataSO.AbilitiesSO, _gameDataSO.SkinsSO, _gameDataSO.PlayerSO, _gameDataSO.WaveSO);
            Container.Bind<GameData>().FromInstance(gameData).AsSingle();
        }
        
        private void BindSound() =>
            Container.Bind<SoundManager>().
                FromInstance(Instantiate(_soundManager)).AsSingle();
        
        private void BindInput()
        { 
            var inputHandler = new GameObject("InputHandler").AddComponent<InputHandlerController>();
            Container.Bind<InputHandlerController>().FromInstance(inputHandler).AsSingle();
        }
    }
}