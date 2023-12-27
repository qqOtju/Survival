using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Project_Assets.Scripts.Infrastructure.Startup.Operations
{
    public class FakeLoadingOperation: ILoading
    {
        private const float TimeToLoad = 7.7f;
        
        public event Action<string> OnDescriptionChange;
        public string Description { get; }
        
        public Task Load(Action<float> onProgress)
        {
            var completionSource = new TaskCompletionSource<bool>();
            async Task LoadScene()
            {
                var time = 0f;
                while (time < TimeToLoad)
                {
                    onProgress?.Invoke(time / TimeToLoad);
                    time += Time.deltaTime;
                    await Task.Yield();
                }
                completionSource.SetResult(true);
            }
            _ = LoadScene();
            return completionSource.Task;
        }
    }
}