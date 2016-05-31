using System;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;

namespace SimpleCMS.DAL
{
    public class BootstrapConfiguration : DbConfiguration
    {
        public BootstrapConfiguration()
        {
            SetProviderServices(SqlProviderServices.ProviderInvariantName, SqlProviderServices.Instance);

            SetDatabaseInitializer(new MigrateDatabaseToLatestVersion<BootstrapContext, Migrations.DALConfiguration>());
            
            //SetDatabaseInitializer(new SimpleCMS.Migrations.Configuration());
            /***
             * 
             * http://msdn.microsoft.com/en-us/data/dn456835
             * 
             * DefaultExecutionStrategy: this execution strategy does not retry any operations, 
             * it is the default for databases other than sql server.
             * 
             * DefaultSqlExecutionStrategy: this is an internal execution strategy that is used 
             * by default. This strategy does not retry at all, however, it will wrap any exceptions 
             * that could be transient to inform users that they might want to enable connection resiliency.
             * 
             * DbExecutionStrategy: this class is suitable as a base class for other execution strategies, 
             * including your own custom ones. It implements an exponential retry policy, 
             * where the initial retry happens with zero delay and the delay increases exponentially 
             * until the maximum retry count is hit. This class has an abstract ShouldRetryOn method 
             * that can be implemented in derived execution strategies to control which exceptions 
             * should be retried.
             * 
             * SqlAzureExecutionStrategy: this execution strategy inherits from DbExecutionStrategy 
             * and will retry on exceptions that are known to be possibly transient when working 
             * with SqlAzure.
             **/
            //SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy(1, TimeSpan.FromSeconds(30)));
            SetExecutionStrategy("System.Data.SqlClient", () => new CustomSqlExecutionStrategy());


        }
    }

    public class CustomSqlExecutionStrategy : DbExecutionStrategy
    {
        /// <summary>
        /// The default retry limit is 5, which means that the total amount of time spent 
        /// between retries is 26 seconds plus the random factor.
        /// </summary>
        public CustomSqlExecutionStrategy()
        {
        }

        /// <summary>
        /// Creates a new instance of "PharylonExecutionStrategy" with the specified limits for
        /// number of retries and the delay between retries.
        /// </summary>
        /// <param name="maxRetryCount"> The maximum number of retry attempts. </param>
        /// <param name="maxDelay"> The maximum delay in milliseconds between retries. </param>
        public CustomSqlExecutionStrategy(int maxRetryCount, TimeSpan maxDelay)
            : base(maxRetryCount, maxDelay)
        {
        }

        protected override bool ShouldRetryOn(Exception ex)
        {
            bool retry = false;

            SqlException sqlException = ex as SqlException;
            if (sqlException != null)
            {
                int[] errorsToRetry =
                {
                    1205,  //Deadlock
                    -2,    //Timeout
                    2601  //primary key violation. Normally you wouldn't want to retry these, 
                          //but some procs in my database can cause it, because it's a crappy 
                          //legacy junkpile.
                };
                if (sqlException.Errors.Cast<SqlError>().Any(x => errorsToRetry.Contains(x.Number)))
                {
                    retry = true;
                }
                else
                {
                    //Add some error logging on this line for errors we aren't retrying.
                    //Make sure you record the Number property of sqlError. 
                    //If you see an error pop up that you want to retry, you can look in 
                    //your log and add that number to the list above.
                }
            }
            if (ex is TimeoutException)
            {
                retry = true;
            }
            return retry;
        }
    }
}