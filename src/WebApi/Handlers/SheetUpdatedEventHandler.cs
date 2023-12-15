using Core.Events;
using Core.Interfaces;
using MediatR;
namespace WebApi.Handllers;
public class SheetUpdatedEventHandler : INotificationHandler<SheetUpdatedEvent>
{
    private readonly IRedisCacheService _redisCacheService;

    public SheetUpdatedEventHandler(IRedisCacheService redisCacheService)
    {
        _redisCacheService = redisCacheService;
    }
    public Task Handle(SheetUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _redisCacheService.RemoveData($"sheet_{notification.SheetId}_{notification.Version}");
        return Task.CompletedTask;
    }
}