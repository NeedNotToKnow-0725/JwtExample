using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtExample.Configs
{
    public class JwtOption
    {
        public const string Name = "JwtOption";
        public string SecretKey { get; set; }
        public int ExpiryMinutes { get; set; }
        public bool ValidateLifetime { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
    }

}
