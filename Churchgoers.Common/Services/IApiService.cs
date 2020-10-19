using Churchgoers.Common.Requests;
using Churchgoers.Common.Responses;
using System;
using System.Threading.Tasks;

namespace Churchgoers.Common.Services
{
    public interface IApiService
    {
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);

        Task<Response> GetList2Async<T>(string urlBase, string servicePrefix, string controller, string token);

        Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);

        Task<Response> RegisterUserAsync(string urlBase, string servicePrefix, string controller, UserRequest userRequest);

        Task<Response> RecoverPasswordAsync(string urlBase, string servicePrefix, string controller, EmailRequest emailRequest);

        Task<Response> ModifyUserAsync(string urlBase, string servicePrefix, string controller, UserRequest userRequest, string token);

        Task<Response> ChangePasswordAsync(string urlBase, string servicePrefix, string controller, ChangePasswordRequest changePasswordRequest, string token);

        Task<Response> PostAsync(string urlBase, string servicePrefix, string controller, DateTime Date, string token);

        Task<Response> PutAsync(string urlBase, string servicePrefix, string controller, MeetingRequest meetingRequest, string token);
    }
}
