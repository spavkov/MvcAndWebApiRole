using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using CloudMockApi.Library.Services.Storage;

namespace CloudMockApi.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ITenantsRepository tenantsRepository;

        public HomeController(ITenantsRepository tenantsRepository)
        {
            this.tenantsRepository = tenantsRepository;
        }

        public ActionResult Index()
        {
            ViewBag.StatusMessage = TempData["StatusMessage"] ?? string.Empty;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddTenant(string tenantId)
        {
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                TempData["StatusMessage"] = "Please supply valid TenantId";
                return RedirectToAction("Index");
            }

            var email = User.Identity.Name;

            if (await this.tenantsRepository.AddUserTenant(email, tenantId))
            {
                TempData["StatusMessage"] = $"Tenant {tenantId} created.";
            }
            else
            {
                TempData["StatusMessage"] = $"Error creating tenant {tenantId}.";
            }
           

            return RedirectToAction("Index");
        }
    }
}