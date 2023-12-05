using Core.IntegrationEvents.Events;
using Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IntegrationEvents.EventHandling
{
    public class SheetUpdatedEventHandler : INotificationHandler<SheetUpdatedEvent>
    {
        private readonly IRedisCacheService _redisCacheService;

        public SheetUpdatedEventHandler(IRedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }
        public Task Handle(SheetUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _redisCacheService.RemoveData($"sheet_{notification.SheetId}");
            return Task.CompletedTask;
        }
    }
}
