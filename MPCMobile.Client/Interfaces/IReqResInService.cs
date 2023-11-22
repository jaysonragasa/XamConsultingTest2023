using MPCMobile.Client.Models;

using System.Threading.Tasks;

namespace MPCMobile.Client.Interfaces
{
    public interface IReqResInService
    {
        Task<bool> Submit(DiaryModel model);
    }
}
