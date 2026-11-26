using Billing.Application.Abstractions.Repositories;
using Billing.Domain.Entities;
using Billing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Billing.Infrastructure.Repositories;

public class PlanRepository : IPlanRepository
{
    private readonly BillingDbContext _ctx;
    public PlanRepository(BillingDbContext ctx) => _ctx = ctx;
    public async Task<Plan?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Plans.FindAsync(new object[] { id }, ct);
    public async Task<Plan?> GetByNameAsync(string name, CancellationToken ct = default)
        => await _ctx.Plans.FirstOrDefaultAsync(p => p.Name == name, ct);
    public async Task AddAsync(Plan entity, CancellationToken ct = default)
        => await _ctx.Plans.AddAsync(entity, ct);
    public void Update(Plan entity) => _ctx.Plans.Update(entity);
    public void Remove(Plan entity) => _ctx.Plans.Remove(entity);
}