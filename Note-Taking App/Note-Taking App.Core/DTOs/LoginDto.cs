using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note_Taking_App.Core.DTOs
{
    public class LoginDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string password { set; get; }

    }
}
