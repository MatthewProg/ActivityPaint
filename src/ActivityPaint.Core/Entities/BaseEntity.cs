﻿namespace ActivityPaint.Core.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public bool IsDeleted { get; set; }
}
