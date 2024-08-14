using CleanArch.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.WebApi.SignalR.UseCases;

public record MarkNotificationAsRead(string Id) : ICommand<Result>;

internal class MarkNotificationAsReadHandler(AppIdentityDbContext context) :
    ICommandHandler<MarkNotificationAsRead, Result>
{
    public async Task<Result> Handle(MarkNotificationAsRead request, CancellationToken cancellationToken)
    {
        await context.Notifications
            .Where(x => x.Id == request.Id)
            .ExecuteUpdateAsync(u => u.SetProperty(p => p.MarkAsRead, true), cancellationToken);

        return Result.Success();
    }
}
