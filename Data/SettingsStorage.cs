using Contracts;
using System.Data.Common;
using System.Data.Entity.SqlServerCompact;
using System.Data.SqlServerCe;
using System.Linq;

namespace Data
{
    public class SettingsStorage
    {
        private void CompilerTrick()
        {
            AddOption.CreateDatabase.HasFlag(AddOption.ExistingDatabase);
            SqlCeFunctions.Pi();
        }

        internal JenkinsContextFactory ContextFactory { get; } = new JenkinsContextFactory();

        public ObserverSettings Settings
        {
            get
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Settings
                        .SingleOrDefault() ?? ObserverSettings.DefaultSettings;
                }
            }
            set
            {
                using (var context = ContextFactory.Create())
                {
                    Clear<ObserverJob>(context);
                    Clear<ObserverServer>(context);
                    Clear<ObserverSettings>(context);

                    context.Settings.Add(value);
                    context.SaveChanges();
                }
            }
        }

        private void Clear<T>(JenkinsObserverContext context) where T : class
        {
            context.Set<T>().RemoveRange(context.Set<T>());
        }
    }
}