using System;
using CloudMockApi.Library.Services.Configuration;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace CloudMockApi.Library.Configuration
{
    public interface ICloudMockApiStorageConfiguration
    {
        string DocumentDbEndpointUrl { get; }
        string DocumentDbDatabaseName { get; }
        string DocumentDbAuthKey { get; }
    }

    public class CloudMockApiStorageConfiguration : ICloudMockApiStorageConfiguration
    {
        private readonly IConfigurationHelper configurationHelper;
        private Lazy<string> documentDbEndpointUrl;
        private Lazy<string> documentDbDatabaseName;
        private Lazy<string> documentDbAuthKey;

        public CloudMockApiStorageConfiguration(IConfigurationHelper configurationHelper)
        {
            this.configurationHelper = configurationHelper;
            documentDbEndpointUrl = new Lazy<string>(() => configurationHelper.GetApplicationSetting("CloudMockApi_Uri"));
            documentDbDatabaseName = new Lazy<string>(() => configurationHelper.GetApplicationSetting("CloudMockApi_Database"));
            documentDbAuthKey = new Lazy<string>(() => configurationHelper.GetApplicationSetting("CloudMockApi_AuthKey"));
        }

        public string DocumentDbEndpointUrl => documentDbEndpointUrl.Value;

        public string DocumentDbDatabaseName => documentDbDatabaseName.Value;

        public string DocumentDbAuthKey => documentDbAuthKey.Value;

    }
}