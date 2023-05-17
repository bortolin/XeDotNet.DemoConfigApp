using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;

namespace XeDotNet.DemoConfigApp
{
    public class SqlServerConfigurationProvider : ConfigurationProvider
    {
        private readonly SqlServerConfigurationSource _configSource;
        private string _query;

        public SqlServerConfigurationProvider(SqlServerConfigurationSource configSource)
        {
            _configSource = configSource;
            _query = $"Select [{_configSource.KeyFieldName}], [{_configSource.ValueFieldName}] from {_configSource.TableName}";

            ChangeToken.OnChange(() => _configSource.Watcher.Watch(), Load);
        }

        public override void Load()
        {
            var settings = new Dictionary<string, string>();

            using (var connection = new SqlConnection(_configSource.ConnectionString))
            {
                var query = new SqlCommand(_query, connection);

                query.Connection.Open();

                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        settings.Add(reader[0].ToString(), reader[1].ToString());
                    }
                }
            }

            Data = settings;
        }
    }
}

