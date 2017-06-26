using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using CloudMockApi.Library.Configuration;
using CloudMockApi.Library.Model.Storage;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace CloudMockApi.Library.Services.Storage
{
    public class TenantsRepository : ITenantsRepository
    {
        private readonly ICloudMockApiStorageConfiguration storageConfiguration;
        private readonly CloudTable tenantsTable;

        public TenantsRepository(ICloudMockApiStorageConfiguration storageConfiguration)
        {
            this.storageConfiguration = storageConfiguration;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                storageConfiguration.StorageConnectionString);

            // Create the tenantsTable client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            this.tenantsTable = tableClient.GetTableReference(this.storageConfiguration.TenantsTableName);
            tenantsTable.CreateIfNotExists();
        }

        public async Task<List<Tenant>> GetUserTenants(string userEmail)
        {
            // Initialize a default TableQuery to retrieve all the entities in the table.
            TableQuery<Tenant> rangeQuery = new TableQuery<Tenant>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, GlobalConstants.TenantPartitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, userEmail.ToLowerInvariant())));

            // Initialize the continuation token to null to start from the beginning of the table.
            TableContinuationToken continuationToken = null;

            var allTenants = new List<Tenant>();
            do
            {
                // Retrieve a segment (up to 1,000 entities).
                TableQuerySegment<Tenant> tableQueryResult =
                    await this.tenantsTable.ExecuteQuerySegmentedAsync(rangeQuery, continuationToken);

                // Assign the new continuation token to tell the service where to
                // continue on the next iteration (or null if it has reached the end).
                continuationToken = tableQueryResult.ContinuationToken;

                allTenants.AddRange(tableQueryResult.Results);

                // Loop until a null continuation token is received, indicating the end of the table.
            } while (continuationToken != null);

            return allTenants;
        }

        public async Task<bool> AddUserTenant(string userEmail, string tenantId)
        {
            var tenant = new Tenant(tenantId, userEmail);

            var insertOperation = TableOperation.Insert(tenant);

            try
            {
                var result = await tenantsTable.ExecuteAsync(insertOperation);
                if (result.HttpStatusCode == (int) HttpStatusCode.NoContent)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Trace.TraceError($"Error while inserting new tenant: {e}");
            }
            return false;
        }

        public async Task<Tenant> FindTenantByTenantId(string tenantId)
        {
            var retrieveOperation = TableOperation.Retrieve<Tenant>(GlobalConstants.TenantPartitionKey, tenantId.ToLowerInvariant());

            var retrievedResult = await this.tenantsTable.ExecuteAsync(retrieveOperation);

            return (Tenant) retrievedResult.Result;
        }
    }

    public interface ITenantsRepository
    {
        Task<List<Tenant>> GetUserTenants(string userEmail);

        Task<bool> AddUserTenant(string userEmail, string tenantId);

        Task<Tenant> FindTenantByTenantId(string tenantId);
    }
}