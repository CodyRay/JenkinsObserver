﻿using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;

namespace Data
{
    public class MyConfiguration : DbConfiguration
    {
        public MyConfiguration()
        {
            //Set the Provider in Code
            SetProviderServices("System.Data.SqlServerCe.4.0", System.Data.Entity.SqlServerCompact.SqlCeProviderServices.Instance);
            //SetExecutionStrategy("System.Data.SqlServerCe.4.0", () => new SqlAzureExecutionStrategy());
            //SetDefaultConnectionFactory(new LocalDbConnectionFactory("v11.0"));
        }
    }
}