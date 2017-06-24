using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CloudMockApi.Admin.Startup))]
namespace CloudMockApi.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
