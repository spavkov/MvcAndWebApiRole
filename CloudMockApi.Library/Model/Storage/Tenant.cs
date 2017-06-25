using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace CloudMockApi.Library.Model.Storage
{
    public class Tenant : TableEntity
    {
        public Tenant()
        {
        }

        public Tenant(string tenantId, string email)
        {
            this.TenantId = tenantId;
            this.PartitionKey = tenantId;
            this.RowKey = tenantId;
            this.Email = email;
        }

        public string TenantId { get; set; }

        public string Email { get; set; }
    }

    public class UserTenant : TableEntity
    {
        public UserTenant()
        {
        }

        public UserTenant(string tenantId, string email)
        {
            this.TenantId = tenantId;
            this.PartitionKey = email;
            this.RowKey = tenantId;
            this.Email = email;
        }

        public string TenantId { get; set; }

        public string Email { get; set; }
    }
}