using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CloudMockApi.Services;

namespace CloudMockApi.Controllers
{
    public class WildcardController : ApiController
    {
        private readonly ITenantsRepository tenantsRepository;

        public WildcardController(ITenantsRepository tenantsRepository)
        {
            this.tenantsRepository = tenantsRepository;
        }

        [HttpPost]
        [HttpGet]
        [AllowAnonymous]
        public Task<HttpResponseMessage> CatchAll()
        {
            var url = this.Request.RequestUri.ToString();
            var resp = new HttpResponseMessage()
            {
                Content = new StringContent($"[{{'Url':'{url}'}}]")
            };

            return Task.FromResult(resp);
        }
    }
}