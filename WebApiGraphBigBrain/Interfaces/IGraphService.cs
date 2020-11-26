using Microsoft.Graph;
using System;
using System.Threading.Tasks;

namespace WebApiGraphBigBrain.Interfaces
{
    public interface IGraphService: IDisposable
    {
        public Task<IGraphServiceClient> GetGraphServiceClient();
    }
}
