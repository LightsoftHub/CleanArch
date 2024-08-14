using CleanArch.Application.Common.Interfaces;

namespace CleanArch.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}
