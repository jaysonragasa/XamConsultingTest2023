using MPCMobile.Client.Helpers;
using MPCMobile.Client.Interfaces;
using MPCMobile.Client.Models;

using System;
using System.Threading.Tasks;

namespace MPCMobile.Client.Services
{
    public class ReqResInService : IReqResInService
    {
        SimpleWebClient _client;

        public ReqResInService()
        {
            _client = new SimpleWebClient();
        }

        public async Task<bool> Submit(DiaryModel model)
        {
            var result = await _client.PostAsync<DiaryModel>("api/users", model);

            return result != null;
        }
    }
}
