using Billing.Domain.Entities;

namespace Billing.Application.Abstractions.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(T entity, CancellationToken ct = default);
    void Update(T entity);
    void Remove(T entity);
}

public interface ISubscriptionRepository : IRepository<Subscription>
{
    Task<Subscription?> GetByTenantAsync(Guid tenantId, CancellationToken ct = default);
}

public interface IPlanRepository : IRepository<Plan>
{
    Task<Plan?> GetByNameAsync(string name, CancellationToken ct = default);
}

public interface ITenantRepository : IRepository<Tenant>
{
}