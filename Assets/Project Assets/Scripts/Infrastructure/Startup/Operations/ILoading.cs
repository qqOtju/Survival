using System;
using Task = System.Threading.Tasks.Task;

namespace Project_Assets.Scripts.Infrastructure.Startup.Operations
{
    public interface ILoading
    {
        public event Action<string> OnDescriptionChange;
        public string Description { get; }

        public Task Load(Action<float> onProgress);
    }
}