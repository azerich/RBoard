using Simulation.Data.Entities.System;
using Simulation.Data.Enums.Site;
using Simulation.Data.Enums.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simulation.Data.Repository.Abstract
{
    public interface ILocalizedMessageRepository
    {
        public Task<string> GetLocalizedMessage(LocaleType locale, MessageEventType messageEvent, string appendix = null);
        public Task<bool> SaveLocalizedMessage(LocalizedMessage entity);
        public Task<List<LocalizedMessage>> GetAllNonEmptyLocalizedMessagesInLocale(LocaleType locale);
        public Task<IQueryable<LocalizedMessage>> GetAllEmptyLocalizedMessagesInLocale(LocaleType locale);
        public Task<bool> DeleteLocalizedMessage(Guid entityId);
    }
}
