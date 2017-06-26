using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using CloudMockApi.Admin.Models;
using CloudMockApi.Library.Model.Storage;
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

        public async Task<ActionResult> Index()
        {
            ViewBag.StatusMessage = TempData["StatusMessage"] ?? string.Empty;

            var email = User.Identity.Name;

            var tenants = await tenantsRepository.GetUserTenants(email);

            var model = new TenantsHomeViewModel()
            {
                Tenants = tenants.OrderBy(t => t.Timestamp).ToList()
            };

            return View(model);
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

        public async Task<ActionResult> EditTenant(string tenantId)
        {
            return new RedirectToRouteResult(new RouteValueDictionary(
              new { action = "Index", controller = "Home" }));
        }

        public async Task<ActionResult> DeleteTenant(string tenantId)
        {
            return new RedirectToRouteResult(new RouteValueDictionary(
              new { action = "Index", controller = "Home" }));
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