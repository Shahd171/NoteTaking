using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note_Taking_App.Core.DTOs
{
    public class CreateNoteDto
    {
        [MaxLength(200)]
        public string Title { get; set; }
        public string Content { get; set; }
        public string? Color { get; set; } // ✅ Add this

        //public string UserId { get; set; }

    }

}
