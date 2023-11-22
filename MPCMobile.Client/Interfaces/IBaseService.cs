using System.Collections.Generic;
using System.Threading.Tasks;

namespace MPCMobile.Client.Interfaces
{
    public interface IBaseService<T>
    {
        Task<List<T>> GetAllAsync();
    }
}
