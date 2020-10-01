using Churchgoers.Common.Responses;
using System.Threading.Tasks;

namespace Churchgoers.Common.Services
{
    public interface IApiService
    {
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);
    }
}
