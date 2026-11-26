using Billing.Application.Abstractions.Repositories;
using Billing.Domain.Entities;
using Billing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Billing.Infrastructure.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly BillingDbContext _ctx;
    public TenantRepository(BillingDbContext ctx) => _ctx = ctx;
    public async Task<Tenant?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Tenants.FindAsync(new object[] { id }, ct);
    public async Task AddAsync(Tenant entity, CancellationToken ct = default)
        => await _ctx.Tenants.AddAsync(entity, ct);
    public void Update(Tenant entity) => _ctx.Tenants.Update(entity);
    public void Remove(Tenant entity) => _ctx.Tenants.Remove(entity);
}