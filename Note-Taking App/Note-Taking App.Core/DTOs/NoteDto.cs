using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note_Taking_App.Core.DTOs
{
    public class NoteDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string Username { get; set; }
        public string? Color { get; set; }

    }

}
