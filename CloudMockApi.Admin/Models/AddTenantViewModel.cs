using System.ComponentModel.DataAnnotations;

namespace CloudMockApi.Admin.Models
{
    public class AddTenantViewModel
    {
        [Required]
        [Display(Name = "TenantId")]
        public string TenantId { get; set; }
    }
}