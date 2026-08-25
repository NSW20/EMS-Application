using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.Domain.Entities
{
    public class AppUser:IdentityUser
    {
        public string? Name { get; set; }
    }
}
