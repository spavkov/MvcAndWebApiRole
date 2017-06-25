using System;
using CloudMockApi.Library.Services.Configuration;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace CloudMockApi.Library.Configuration
{
    public interface ICloudMockApiStorageConfiguration
    {
        string TenantsTableName { get; }

        string StorageConnectionString { get; }

        string UserTenantsTableName { get; }
    }

    public class CloudMockApiStorageConfiguration : ICloudMockApiStorageConfiguration
    {
        private readonly IConfigurationHelper configurationHelper;
        private Lazy<string> tenantsTableName;
        private Lazy<string> userTenantsTableName;
        private Lazy<string> storageConnectionString;

        public CloudMockApiStorageConfiguration(IConfigurationHelper configurationHelper)
        {
            this.configurationHelper = configurationHelper;
            
            storageConnectionString  = new Lazy<string>(() => configurationHelper.GetApplicationSetting("CloudMockApi.StorageConnectionString"));
            tenantsTableName = new Lazy<string>(() => configurationHelper.GetApplicationSetting("CloudMockApi.TenantsTableName"));
            userTenantsTableName = new Lazy<string>(() => configurationHelper.GetApplicationSetting("CloudMockApi.UserTenantsTableName"));
        }

        public string TenantsTableName => tenantsTableName.Value;

        public string UserTenantsTableName => userTenantsTableName.Value;

        public string StorageConnectionString => storageConnectionString.Value;

    }
}