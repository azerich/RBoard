using Microsoft.AspNetCore.Http;
using Simulation.Data.Enums.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Data.System
{
    public sealed class SiteConfiguration
    {
        public static string ConnectionString { get; set; }
        public static int PasswordMinLength { get; set; }
        public static string DefaultLocale { get; set; }
        public static LocaleType Locale { get; set; }
    }
}
