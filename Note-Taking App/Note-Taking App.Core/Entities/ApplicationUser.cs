using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Note_Taking_App.Infrastructure;
namespace Note_Taking_App.Core.Entities
{
    public class ApplicationUser:IdentityUser
    {


        public ICollection<Note>? Notes { get; set; }
    }
}
