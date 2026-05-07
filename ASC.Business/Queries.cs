using ASC.Model.Models;
using ASC.Utilities;
using System;
using System.Linq.Expressions;

namespace ASC.Business
{
    public static class Queries
    {
        public static Expression<Func<ServiceRequest, bool>> ServiceRequestByCustomer(string customerId)
        {
            var predicate = PredicateBuilder.True<ServiceRequest>();
            predicate = predicate.And(r => r.CustomerCode == customerId);
            predicate = predicate.And(r => !r.IsDeleted);
            return predicate;
        }

        public static Expression<Func<ServiceRequest, bool>> ServiceRequestByEngineer(string engineerId)
        {
            var predicate = PredicateBuilder.True<ServiceRequest>();
            predicate = predicate.And(r => r.ServiceEngineer == engineerId);
            predicate = predicate.And(r => !r.IsDeleted);
            return predicate;
        }

        public static Expression<Func<ServiceRequest, bool>> AllServiceRequests()
        {
            var predicate = PredicateBuilder.True<ServiceRequest>();
            predicate = predicate.And(r => !r.IsDeleted);
            return predicate;
        }
    }
}