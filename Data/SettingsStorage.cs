using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.SqlServerCompact;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace Data
{
    public class SettingsStorage
    {
        private void CompilerTrick()
        {
            AddOption.CreateDatabase.HasFlag(AddOption.ExistingDatabase);
            SqlCeFunctions.Pi();
        }

        protected DbConnection GetNewConnection()
        {
            return
                new SqlCeConnection(
                    @"Data Source=TestEntity.sdf;File Mode=Exclusive;Persist Security Info=False;Password=test");
        }

        public ObserverSettings Settings
        {
            get
            {
                using (var context = new JenkinsObserverContext(GetNewConnection()))
                {
                    return context.Settings
                        .SingleOrDefault() ?? ObserverSettings.DefaultSettings;
                }
            }
            set
            {
                using (var context = new JenkinsObserverContext(GetNewConnection()))
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
