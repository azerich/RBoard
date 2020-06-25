using Simulation.Data.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Data.Repository
{
    public class DataManager
    {
        public ILocalizedMessageRepository LocalizedMessages { get; set; }
        public DataManager(ILocalizedMessageRepository localizedMessage)
        {
            LocalizedMessages = localizedMessage;
        }
    }
}
