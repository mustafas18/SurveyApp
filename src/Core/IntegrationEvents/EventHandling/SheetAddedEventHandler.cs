using Core.IntegrationEvents.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IntegrationEvents.EventHandling
{
    public class SheetAddedEventHandler : INotificationHandler<SheetAddedEvent>
    {
        private readonly ILogger<SheetAddedEventHandler> _logger;

        public SheetAddedEventHandler(ILogger<SheetAddedEventHandler> logger)
        {
            _logger = logger;
        }
        public async Task Handle(SheetAddedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling integration event: {notification.SheetId} is added");

            // TODO: do meaniingful work here
            await Task.Delay(1);

        }
    }
}
