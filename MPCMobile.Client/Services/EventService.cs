using MPCMobile.Client.Interfaces;
using MPCMobile.Client.Models;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MPCMobile.Client.Services
{
    public class EventService : IEventService<EventModel>
    {
        public EventService() { }

        public async Task<List<EventModel>> GetAllAsync()
        {
            var list = new List<EventModel>();

            // dummy data
            for (int i = 0; i < 5; i++)
                list.Add(new EventModel()
                {
                    Id = Guid.NewGuid(),
                    EventName = $"Event {(i + 1)}"
                });

            await Task.CompletedTask;

            return list;
        }
    }
}
