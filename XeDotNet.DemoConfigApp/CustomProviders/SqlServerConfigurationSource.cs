namespace XeDotNet.DemoConfigApp
{
    public class SqlServerConfigurationSource : IConfigurationSource
    {
        public string ConnectionString { get; set; }

        public string TableName { get; set; }
        public string KeyFieldName { get; set; }
        public string ValueFieldName { get; set; }

        public SqlServerWatcher Watcher { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new SqlServerConfigurationProvider(this);
        }
    }
    
}