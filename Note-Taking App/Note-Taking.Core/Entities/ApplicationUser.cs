using Microsoft.AspNetCore.Identity;
using Microsoft.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Note_Taking.Core.Entities
{
    internal class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        // Navigation Property
       // public ICollection<Note> Notes { get; set; }
    }
}
