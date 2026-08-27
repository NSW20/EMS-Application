using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.DTOs
{

        public class ResetPasswordDTO
        {
            public string? Email { get; set; }          
            public string? Token { get; set; }        
            public string? NewPassword { get; set; }   
            public string? ConfirmPassword { get; set; }
        }

}
