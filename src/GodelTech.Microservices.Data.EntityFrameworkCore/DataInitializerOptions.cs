namespace GodelTech.Microservices.Data.EntityFrameworkCore
{
    /// <summary>
    /// Data initializer options.
    /// </summary>
    public class DataInitializerOptions
    {
        /// <summary>
        /// The name of connection string.
        /// </summary>
        public string ConnectionStringName { get; set; } = "DefaultConnection";

        /// <summary>
        /// The SQL server health check name.
        /// </summary>
        public string SqlServerHealthCheckName { get; set; } = "sqlserver";

        /// <summary>
        /// Enables database migration on Startup.
        /// </summary>
        public bool EnableDatabaseMigration { get; set; }
    }
}
