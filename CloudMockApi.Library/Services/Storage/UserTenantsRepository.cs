using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CloudMockApi.Library.Configuration;
using CloudMockApi.Library.Model.Storage;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;

namespace CloudMockApi.Library.Services.Storage
{
    public class TenantsRepository : ITenantsRepository
    {
        private const string TenantsCollectionId = "Tenants";
        private readonly ICloudMockApiStorageConfiguration storageConfiguration;
        private readonly DocumentClient client;

        public TenantsRepository(ICloudMockApiStorageConfiguration storageConfiguration)
        {
            this.storageConfiguration = storageConfiguration;
            this.client = new DocumentClient(new Uri(this.storageConfiguration.DocumentDbEndpointUrl), this.storageConfiguration.DocumentDbAuthKey);
        }

        public async Task<List<Tenant>> GetUserTenants(string userEmail)
        {
            var emailToLower = userEmail.ToLowerInvariant();
            // Query across partition keys
            var query = client.CreateDocumentQuery<Tenant>(
                UriFactory.CreateDocumentCollectionUri(this.storageConfiguration.DocumentDbDatabaseName, TenantsCollectionId),
                new FeedOptions {EnableCrossPartitionQuery = true})
                .Where(m => m.Email == emailToLower).AsDocumentQuery();

            var tenants = new List<Tenant>();
            while (query.HasMoreResults)
            {
                var tenantFeedResponse = await query.ExecuteNextAsync<Tenant>();
                tenants.AddRange(tenantFeedResponse.ToList());
            }

            return tenants;
        }

        public async Task AddUserTenant(string userEmail, string tenantId)
        {
            var tenant = new Tenant()
            {
                Id = tenantId,
                Email = userEmail,
                TenantId = tenantId
            };

            await client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(this.storageConfiguration.DocumentDbDatabaseName, TenantsCollectionId),
                tenant);
        }

        public async Task<Tenant> FindTenantByTenantId(string tenantId)
        {
            var emailToLower = tenantId.ToLowerInvariant();
            // Query across partition keys
            var query = client.CreateDocumentQuery<Tenant>(
                UriFactory.CreateDocumentCollectionUri(this.storageConfiguration.DocumentDbDatabaseName, TenantsCollectionId),
                new FeedOptions { EnableCrossPartitionQuery = true })
                .Where(m => m.TenantId == emailToLower).AsDocumentQuery();

            if (!query.HasMoreResults)
            {
                return null;
            }

            var tenantFeedResponse = await query.ExecuteNextAsync<Tenant>();
            return tenantFeedResponse.FirstOrDefault();
        }
    }

    public interface ITenantsRepository
    {
        Task<List<Tenant>> GetUserTenants(string userEmail);

        Task AddUserTenant(string userEmail, string tenantId);

        Task<Tenant> FindTenantByTenantId(string tenantId);
    }
}