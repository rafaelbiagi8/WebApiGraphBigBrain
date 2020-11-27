using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiGraphBigBrain.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<string> GetUser(string filter = null, string value = null);
        Task<string> GetUserMap();
    }
}
