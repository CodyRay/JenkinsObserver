using Contracts;
using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace Data
{
    internal class JenkinsObserverContext : DbContext
    {
        public JenkinsObserverContext(DbConnection connection)
            : base(connection, true)
        { }

        public DbSet<ObserverSettings> Settings { get; set; }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var errorMessage =
                    dbEx.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors)
                        .Aggregate("",
                            (current, validationError) =>
                                current +
                                (Environment.NewLine +
                                 string.Format("Property: {0} Error: {1}", validationError.PropertyName,
                                     validationError.ErrorMessage)));

                throw new Exception(errorMessage, dbEx);
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}