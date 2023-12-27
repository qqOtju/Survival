using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Project_Assets.Scripts.Infrastructure.Startup.Loading;
using Project_Assets.Scripts.Infrastructure.Startup.Operations;
using UnityEngine;
using UnityEngine.SceneManagement;
using ILoading = Project_Assets.Scripts.Infrastructure.Startup.Operations.ILoading;

namespace Project_Assets.Scripts.Infrastructure.Startup
{
    public class AppStartup : MonoBehaviour
    {
        [Header("Menu Scene")] 
        [SerializeField] private string _menuScene;
        [Header("Loading Screen")]
        [SerializeField] private LoadingScreen _loading;
        
        public event Action OnLoaded;
        
        private async void Awake()
        {
            var loadingOperations = new Queue<ILoading>();
            loadingOperations.Enqueue(new SceneLoading(_menuScene, LoadSceneMode.Additive));
            await Load(loadingOperations);
            OnLoaded?.Invoke();
            Destroy(gameObject);
        }

        private async Task Load(Queue<ILoading> loadingOperations)
        {
            var loadingScreen = Instantiate(_loading);
            await loadingScreen.Load(loadingOperations);
            Destroy(loadingScreen.gameObject);
        }
    }
}