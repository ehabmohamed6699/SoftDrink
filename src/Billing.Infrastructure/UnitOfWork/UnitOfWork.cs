using Billing.Application.Abstractions;
using Billing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Billing.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly BillingDbContext _ctx;
    public UnitOfWork(BillingDbContext ctx) => _ctx = ctx;
    public Task<int> CommitAsync(CancellationToken ct = default) => _ctx.SaveChangesAsync(ct);
}