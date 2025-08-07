using Note_Taking_App.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Note_Taking_App.Infrastructure;

public partial class Note
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UserId { get; set; }
            public string? Color { get; set; } // ✅ Add this



}
