using System.Data.Entity.Infrastructure;
using System.Data.SqlServerCe;

namespace Data
{
    internal class JenkinsContextFactory : IDbContextFactory<JenkinsObserverContext>
    {
        public string Name { get; } = "JenkinsObserverData";

        public JenkinsObserverContext Create()
        {
            return new JenkinsObserverContext(GetSqlCeConnection());
        }

        public SqlCeConnection GetSqlCeConnection()
        {
            return new SqlCeConnection($@"Data Source={ Name }.sdf;File Mode=Exclusive;Persist Security Info=False;");
        }
    }
}