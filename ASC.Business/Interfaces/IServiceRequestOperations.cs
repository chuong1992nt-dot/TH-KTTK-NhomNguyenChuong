using ASC.Model.Models;

namespace ASC.Business.Interfaces
{
    public interface IServiceRequestOperations
    {
        Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequest serviceRequest);
        Task<ServiceRequest?> GetServiceRequestByRowKeyAsync(string rowKey);
        Task<List<ServiceRequest>> GetServiceRequestsAsync();
        Task<List<ServiceRequest>> GetAllServiceRequestsAsync();
        Task<List<ServiceRequest>> GetServiceRequestsByCustomerAsync(string customerCode);
        Task<List<ServiceRequest>> GetServiceRequestsByEngineerAsync(string engineerEmail);
        Task<bool> UpdateServiceRequestAsync(ServiceRequest serviceRequest);
    }
}
