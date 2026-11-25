using Billing.Domain.Entities;
using Billing.Domain.Enums;

namespace Billing.Domain.Proration;

public static class ProrationCalculator
{
    public static decimal Compute(
        Plan oldPlan,
        Plan newPlan,
        BillingCycle cycle,
        DateTime periodStart,
        DateTime periodEnd,
        DateTime changeDate)
    {
        var totalDays = (periodEnd - periodStart).TotalDays;
        if (totalDays <= 0) return 0m;

        var remainderDays = (periodEnd - changeDate).TotalDays;
        if (remainderDays <= 0) return 0m;

        var oldPrice = cycle == BillingCycle.Monthly ? oldPlan.MonthlyPrice : oldPlan.AnnualPrice;
        var newPrice = cycle == BillingCycle.Monthly ? newPlan.MonthlyPrice : newPlan.AnnualPrice;

        var oldDaily = oldPrice / (decimal)totalDays;
        var newDaily = newPrice / (decimal)totalDays;

        var credit = oldDaily * (decimal)remainderDays;
        var charge = newDaily * (decimal)remainderDays;

        var net = charge - credit;
        return Math.Round(net, 2, MidpointRounding.AwayFromZero);
    }
}