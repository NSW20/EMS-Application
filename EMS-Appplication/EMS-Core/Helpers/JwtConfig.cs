using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.Helpers
{
    public class JwtConfig
    {
        public string? key { get; set; }
        public string? audience { get; set; }
        public string? issuer { get; set; }
    }
}
