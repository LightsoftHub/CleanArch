﻿namespace CleanArch.eCode.Infrastructure.Identity.Models;

public class Notification : Light.Domain.Entities.Default.AuditableEntity
{
    public string FromUserId { get; set; } = null!;

    public string? FromName { get; set; }

    public string ToUserId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Message { get; set; }

    public string? Url { get; set; }

    public bool MarkAsRead { get; set; }
}