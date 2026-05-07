using ASC.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASC.Business.Interfaces
{
    public interface IServiceRequestOperations
    {
        Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequest serviceRequest);
        Task<List<ServiceRequest>> GetServiceRequestsAsync();
        Task<List<ServiceRequest>> GetServiceRequestsByCustomerAsync(string customerId);
        Task<List<ServiceRequest>> GetServiceRequestsByEngineerAsync(string engineerId);
        Task<List<ServiceRequest>> GetAllServiceRequestsAsync();
    }
}