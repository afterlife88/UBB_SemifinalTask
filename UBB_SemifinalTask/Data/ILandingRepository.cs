using System;
using System.Threading.Tasks;
using UBB_SemifinalTask.Models;

namespace UBB_SemifinalTask.Data
{
    public interface ILandingRepository : IDisposable
    {
        Task<string> GetStylesAsync(string authorId);
        Task<LandingResources> AddStylesAsync(LandingResources item);
        Task<LandingResources> AddContentAsync(LandingResources item);
        Task<string> GetContentAsync(string authorId);
        Task<LandingResources> GetPageAsync(int id);
        Task<MoneyReturnModel> GetMoney(int id);
    }
}
