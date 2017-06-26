using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace CloudMockApi.Library.Model.Storage
{
    public class Tenant : TableEntity
    {
        public Tenant()
        {
        }

        public Tenant(string tenantId, string email)
        {
            this.PartitionKey = GlobalConstants.TenantPartitionKey;
            this.RowKey = tenantId?.ToLowerInvariant() ?? string.Empty;
            this.Email = email?.ToLowerInvariant();
        }

        public string Email { get; set; }
    }
}