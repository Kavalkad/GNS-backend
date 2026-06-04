using System.Linq.Expressions;
using GNS.Data.Entities;



namespace GNS.Data.Repositories.Interfaces
{
    public interface IEmployeesRepository : IRepository<EmployeeEntity>
    {
        Task<List<EmployeeEntity>> GetWithDetailsByExpressionAsync(Expression<Func<EmployeeEntity, bool>> predicate, CancellationToken token = default);
        Task SetZeroBonusesAsync(CancellationToken token = default);
        Task SetZeroPenaltiesAsync(CancellationToken token = default);
    }
}