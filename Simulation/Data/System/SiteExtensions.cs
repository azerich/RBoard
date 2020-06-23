using Microsoft.CodeAnalysis.CSharp.Syntax;
using Simulation.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Data.System
{
    public static class SiteExtensions
    {
        public static string CutController(this string controllerName)
        {
            return controllerName.Replace("Controller", "");
        }
    }
}
