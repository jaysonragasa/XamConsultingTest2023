using MPCMobile.Client.Interfaces;
using MPCMobile.Client.Models;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MPCMobile.Client.Services
{
    public class AreaService : IAreaService<AreaModel>
    {
        public AreaService() { }

        public async Task<List<AreaModel>> GetAllAsync()
        {
            var list = new List<AreaModel>();

            // dummy data
            for (int i = 0; i < 5; i++)
                list.Add(new AreaModel()
                {
                    Id = Guid.NewGuid(),
                    AreaName = $"Area {(i + 1)}"
                });

            await Task.CompletedTask;

            return list;
        }
    }
}
