using System.Data.Entity;
using Contracts;
using System.Data.Entity.SqlServerCompact;
using System.Data.SqlServerCe;
using System.Linq;
using Newtonsoft.Json;

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
                        .Include("Servers")
                        .Include("Servers.Jobs")
                        .SingleOrDefault() ?? ObserverSettings.DefaultSettings;
                }
            }
            set
            {
                using (var context = ContextFactory.Create())
                {
                    ClearData(context);

                    context.Settings.Add(value);
                    context.SaveChanges();
                }
            }
        }

        public void ClearData()
        {
            using (var context = ContextFactory.Create())
            {
                ClearData(context);
            }
        }

        private void ClearData(JenkinsObserverContext context)
        {
            Clear<ObserverJob>(context);
            Clear<ObserverServer>(context);
            Clear<ObserverSettings>(context);
        }

        public string SettingsAsJson
        {
            get { return JsonConvert.SerializeObject(Settings); }
            set { Settings = JsonConvert.DeserializeObject<ObserverSettings>(value); }
        }

        private static void Clear<T>(DbContext context) where T : class
        {
            context.Set<T>().RemoveRange(context.Set<T>());
        }

        public void DeleteDatabase()
        {
            using (var context = ContextFactory.Create())
            {
                context.Database.Delete();
            }
        }
    }
}