using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CloudMockApi.Library.Configuration;
using CloudMockApi.Library.Model.Storage;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace CloudMockApi.Library.Services.Storage
{
    public class TenantsRepository : ITenantsRepository
    {
        private readonly ICloudMockApiStorageConfiguration storageConfiguration;
        private readonly CloudTable tenantsTable;
        private CloudTable userTenantsTable;

        public TenantsRepository(ICloudMockApiStorageConfiguration storageConfiguration)
        {
            this.storageConfiguration = storageConfiguration;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                storageConfiguration.StorageConnectionString);

            // Create the tenantsTable client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            this.tenantsTable = tableClient.GetTableReference(this.storageConfiguration.TenantsTableName);
            tenantsTable.CreateIfNotExists();

            this.userTenantsTable = tableClient.GetTableReference(this.storageConfiguration.UserTenantsTableName);
            tenantsTable.CreateIfNotExists();
        }

        public async Task<List<Tenant>> GetUserTenants(string userEmail)
        {
            var emailToLower = userEmail.ToLowerInvariant();
            // Query across partition keys
            var tenants = new List<Tenant>();

            return tenants;
        }

        public async Task<bool> AddUserTenant(string userEmail, string tenantId)
        {
            var tenant = new Tenant(tenantId, userEmail);

            var insertOperation = TableOperation.Insert(tenant);

            try
            {
                var result = await tenantsTable.ExecuteAsync(insertOperation);
                if (result.HttpStatusCode != (int) HttpStatusCode.NoContent)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Trace.TraceError($"Error while inserting new tenant: {e}");
                return false;
            }

            try
            {
                var userTenant = new UserTenant(tenantId, userEmail);

                var userTenantInsertOperation = TableOperation.Insert(userTenant);
                var result = await userTenantsTable.ExecuteAsync(userTenantInsertOperation);
                return result.HttpStatusCode == (int)HttpStatusCode.NoContent;
            }
            catch (Exception e)
            {
                Trace.TraceError($"Error while inserting new user tenant: {e}");
                return false;
            }
        }

        public Task<Tenant> FindTenantByTenantId(string tenantId)
        {
            var emailToLower = tenantId.ToLowerInvariant();
            // Query across partition keys

            return Task.FromResult<Tenant>(null);
        }
    }

    public interface ITenantsRepository
    {
        Task<List<Tenant>> GetUserTenants(string userEmail);

        Task<bool> AddUserTenant(string userEmail, string tenantId);

        Task<Tenant> FindTenantByTenantId(string tenantId);
    }
}