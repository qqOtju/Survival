using System.Collections.Generic;
using System.Threading.Tasks;
using Project_Assets.Scripts.Infrastructure.Startup.Operations;

namespace Project_Assets.Scripts.Infrastructure.Startup.Loading
{
    public interface ILoading
    {
        public Task Load(Queue<Operations.ILoading> loadingOperations, bool withProgressBar = true);
    }
}