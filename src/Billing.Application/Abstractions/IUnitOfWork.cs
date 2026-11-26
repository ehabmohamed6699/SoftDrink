namespace Billing.Application.Abstractions;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken ct = default);
}

public interface ITenantContext
{
    Guid TenantId { get; }
}