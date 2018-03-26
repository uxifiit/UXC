using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using UXC.Devices.ExternalEvents.Entities;
using UXC.Devices.ExternalEvents.Services;

namespace UXC.Devices.ExternalEvents.Hubs
{
    public class EventHub : Hub
    {
        private readonly EventService _service;

        public EventHub(EventService service)
        {
            _service = service;
        }

        public bool Send(ExternalEvent @event)
        {
            try
            {
                _service.Record(@event);
                return true;
            }
            catch (ArgumentException ex)
            {
                return false;
            }
        }

        public bool SendMany(List<ExternalEvent> events)
        {
            try
            {
                _service.RecordMany(events);
                return true;
            }
            catch (ArgumentException ex)
            {
                return false;
            }
        }
    }
}
