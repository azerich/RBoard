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
        public Task<string> GetLocalizedWord(LocaleType locale, StringWords word, string appendix = null);
        public Task<string> GetLocalizedSentence(LocaleType locale, StringSentences sentence, string appendix = null);
        public Task<bool> SaveLocalizedMessage(LocalizedMessage entity);
        public Task<bool> DeleteLocalizedMessage(Guid entityId);
    }
}
