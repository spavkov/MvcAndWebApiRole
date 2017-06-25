using System.Configuration;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace CloudMockApi.Library.Services.Configuration
{
    public interface IConfigurationHelper
    {
        string GetApplicationSetting(string name);
    }

    public class ConfigurationHelper : IConfigurationHelper
    {
        public string GetApplicationSetting(string name)
        {
            if (ConfigurationManager.AppSettings[name] != null)
            {
                return ConfigurationManager.AppSettings[name];
            }

            return RoleEnvironment.GetConfigurationSettingValue(name);
        }
    }
}