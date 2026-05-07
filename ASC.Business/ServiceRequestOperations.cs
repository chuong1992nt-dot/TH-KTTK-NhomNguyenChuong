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
    }
}