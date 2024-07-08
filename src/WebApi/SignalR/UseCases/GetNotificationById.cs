using CleanArch.eCode.Infrastructure.Identity;
using CleanArch.eCode.Shared.Notifications;
using Light.EntityFrameworkCore.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.eCode.WebApi.SignalR.UseCases;

public record GetNotificationById(string Id) : IQuery<Result<NotificationDto>>;

internal class GetNotificationByIdHandler(AppIdentityDbContext context) :
    IQueryHandler<GetNotificationById, Result<NotificationDto>>
{
    public async Task<Result<NotificationDto>> Handle(GetNotificationById request,
        CancellationToken cancellationToken)
    {
        return await context.Notifications
            .Where(x => x.Id == request.Id)
            .AsNoTracking()
            .ProjectToType<NotificationDto>()
            .SingleResultAsync("Notification", request.Id, cancellationToken);
    }
}
