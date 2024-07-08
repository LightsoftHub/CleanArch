using CleanArch.eCode.Application.Common.Interfaces;

namespace CleanArch.eCode.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}
