using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note_Taking_App.Core.DTOs
{
    public class UserDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string FullName { set; get; }
        public string password { set; get; }
    }
}
