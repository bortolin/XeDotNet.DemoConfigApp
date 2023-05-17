namespace XeDotNet.DemoConfigApp
{
    public static class SqlServerConfigurationExtensions
    {
        public static IConfigurationBuilder AddSqlServer(this IConfigurationBuilder builder, string connectionString, string tableName, string keyFiledName, string valueFieldName)
        {
            return builder.Add(new SqlServerConfigurationSource { ConnectionString = connectionString,
                                                                    TableName = tableName,
                                                                    KeyFieldName = keyFiledName,
                                                                    ValueFieldName = valueFieldName,
                                                                    Watcher = new SqlServerWatcher(TimeSpan.FromSeconds(10)) });
        }
    }
}