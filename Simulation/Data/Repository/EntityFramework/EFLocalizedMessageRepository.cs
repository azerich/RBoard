using Microsoft.EntityFrameworkCore;
using Simulation.Data.Entities.System;
using Simulation.Data.Enums.Site;
using Simulation.Data.Enums.Users;
using Simulation.Data.Repository.Abstract;
using Simulation.Data.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simulation.Data.Repository.EntityFramework
{
    public class EFLocalizedMessageRepository : ILocalizedMessageRepository
    {
        private readonly SiteDbContext context;
        public EFLocalizedMessageRepository(SiteDbContext context) => this.context = context;

        public async Task<string> GetLocalizedWord(LocaleType locale, StringWords word, string appendix = null)
        {
            LocalizedMessage result = await context.LocalizedMessages.FirstOrDefaultAsync(entity => entity.Locale == locale && entity.Word == word).ConfigureAwait(false);

            if (string.IsNullOrEmpty(result.Message)) return "This message is not localized";
            else return appendix == null ? result.Message : result.Message + " " + appendix;
        }
        public async Task<string> GetLocalizedSentence(LocaleType locale, StringSentences sentence, string appendix = null)
        {
            LocalizedMessage result = await context.LocalizedMessages.FirstOrDefaultAsync(entity => entity.Locale == locale && entity.Sentence == sentence).ConfigureAwait(false);

            if (string.IsNullOrEmpty(result.Message)) return "This message is not localized";
            else if (appendix == " ") return result.Message + appendix;
            else return appendix == null ? result.Message : result.Message + " " + appendix;
        }
        public async Task<bool> SaveLocalizedMessage(LocalizedMessage entity)
        {
            if (entity.Id == default)
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> DeleteLocalizedMessage(Guid entityId)
        {
            context.LocalizedMessages.Remove(new LocalizedMessage() { Id = entityId });
            try
            {
                await context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
