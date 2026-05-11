using ASC.Business.Interfaces;
using ASC.DataAccess.Interfaces;
using ASC.Model.Models;

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

        public async Task<ServiceRequest?> GetServiceRequestByRowKeyAsync(string rowKey)
        {
            return await _unitOfWork.Repository<ServiceRequest>().FindAsync(rowKey);
        }

        public async Task<List<ServiceRequest>> GetServiceRequestsAsync()
        {
            var all = await _unitOfWork.Repository<ServiceRequest>().GetAllAsync();
            return all.Where(r => !r.IsDeleted).OrderByDescending(r => r.CreatedDate).ToList();
        }

        public async Task<List<ServiceRequest>> GetAllServiceRequestsAsync()
        {
            var all = await _unitOfWork.Repository<ServiceRequest>().GetAllAsync();
            return all.Where(r => !r.IsDeleted).OrderByDescending(r => r.CreatedDate).ToList();
        }

        public async Task<List<ServiceRequest>> GetServiceRequestsByCustomerAsync(string customerCode)
        {
            var all = await _unitOfWork.Repository<ServiceRequest>()
                .FindAllByAsync(r => r.CustomerCode == customerCode && !r.IsDeleted);
            return all.OrderByDescending(r => r.CreatedDate).ToList();
        }

        public async Task<List<ServiceRequest>> GetServiceRequestsByEngineerAsync(string engineerEmail)
        {
            var all = await _unitOfWork.Repository<ServiceRequest>()
                .FindAllByAsync(r => r.ServiceEngineer == engineerEmail && !r.IsDeleted);
            return all.OrderByDescending(r => r.CreatedDate).ToList();
        }

        public async Task<bool> UpdateServiceRequestAsync(ServiceRequest serviceRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<ServiceRequest>();
                repo.Update(serviceRequest);
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
