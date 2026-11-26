using Billing.Application.Abstractions.Repositories;
using Billing.Application.Result;
using Billing.Domain.Entities;
using Billing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Billing.Infrastructure.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly BillingDbContext _ctx;
    private readonly IResultBuilder _rb;
    public SubscriptionRepository(BillingDbContext ctx, IResultBuilder rb) { _ctx = ctx; _rb = rb; }

    public async Task<Subscription?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Subscriptions.FindAsync(new object[] { id }, ct);

    public async Task<Subscription?> GetByTenantAsync(Guid tenantId, CancellationToken ct = default)
        => await _ctx.Subscriptions.FirstOrDefaultAsync(s => s.TenantId == tenantId, ct);

    public async Task AddAsync(Subscription entity, CancellationToken ct = default)
        => await _ctx.Subscriptions.AddAsync(entity, ct);

    public void Update(Subscription entity) => _ctx.Subscriptions.Update(entity);
    public void Remove(Subscription entity) => _ctx.Subscriptions.Remove(entity);
}