using Simulation.Data.Enums.Site;
using Simulation.Data.Enums.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Data.Entities.System
{
    public class LocalizedMessage
    {
        public Guid Id { get; set; }
        public LocaleType Locale { get; set; }
        public StringWords Word { get; set; }
        public StringSentences Sentence { get; set; }
        public string Message { get; set; } = "Not localized";
    }
}
