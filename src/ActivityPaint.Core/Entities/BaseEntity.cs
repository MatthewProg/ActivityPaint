﻿using System.ComponentModel.DataAnnotations;

namespace ActivityPaint.Core.Entities;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
}
