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

        public async Task<string> GetLocalizedMessage(LocaleType locale, MessageEventType messageEvent, string appendix = null)
        {
            LocalizedMessage result = await context.LocalizedMessages.FirstOrDefaultAsync(entity => entity.Locale == locale && entity.MessageEvent == messageEvent).ConfigureAwait(false);

            if (string.IsNullOrEmpty(result.Message)) return "This message is not localized";
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
        public async Task<List<LocalizedMessage>> GetAllNonEmptyLocalizedMessagesInLocale(LocaleType locale)
        {
            return await context.LocalizedMessages.Where(entity => entity.Locale == locale).ToListAsync().ConfigureAwait(false);
        }
        public async Task<IQueryable<LocalizedMessage>> GetAllEmptyLocalizedMessagesInLocale(LocaleType locale)
        {
            List<LocalizedMessage> result = new List<LocalizedMessage>();
            foreach (MessageEventType item in Enum.GetValues(typeof(MessageEventType)))
            {
                LocalizedMessage localizedMessage = await context.LocalizedMessages.FirstOrDefaultAsync(entity => entity.Locale == locale && entity.MessageEvent == item).ConfigureAwait(false);
                if (localizedMessage == null)
                {
                    LocalizedMessage entry = new LocalizedMessage()
                    {
                        Locale = locale,
                        MessageEvent = item,
                        Message = "This message is not localized"
                    };
                    result.Add(entry);
                }
            }
            return (IQueryable<LocalizedMessage>)result;
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
