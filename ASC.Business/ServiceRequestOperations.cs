using ASC.Business.Interfaces;
using ASC.DataAccess.Interfaces;
using ASC.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASC.Business
{
    public class ServiceRequestOperations : IServiceRequestOperations
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceRequestOperations(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequest serviceRequest)
        {
            var repository = _unitOfWork.Repository<ServiceRequest>();
            await repository.AddAsync(serviceRequest);
            await _unitOfWork.CommitTransactionAsync();
            return serviceRequest;
        }

        public async Task<List<ServiceRequest>> GetServiceRequestsAsync()
        {
            var repository = _unitOfWork.Repository<ServiceRequest>();
            var requests = await repository.GetAllAsync();
            return requests.ToList();
        }

        // Thêm 3 method còn thiếu
        public async Task<List<ServiceRequest>> GetAllServiceRequestsAsync()
        {
            var repository = _unitOfWork.Repository<ServiceRequest>();
            var requests = await repository.GetAllAsync();
            return requests.Where(r => !r.IsDeleted).ToList();
        }

        public async Task<List<ServiceRequest>> GetServiceRequestsByCustomerAsync(string customerId)
        {
            var repository = _unitOfWork.Repository<ServiceRequest>();
            var requests = await repository.GetAllAsync();
            return requests.Where(r => r.CustomerCode == customerId && !r.IsDeleted).ToList();
        }

        public async Task<List<ServiceRequest>> GetServiceRequestsByEngineerAsync(string engineerId)
        {
            var repository = _unitOfWork.Repository<ServiceRequest>();
            var requests = await repository.GetAllAsync();
            return requests.Where(r => r.ServiceEngineer == engineerId && !r.IsDeleted).ToList();
        }
    }
}