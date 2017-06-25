using System.Collections.Generic;
using Newtonsoft.Json;

namespace CloudMockApi.Library.Model.Storage
{
    public class Tenant
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("tenantId")]
        public string TenantId { get; set; }

        [JsonProperty("userEmail")]
        public string Email { get; set; }
    }
}