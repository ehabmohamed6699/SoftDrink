using Billing.Domain.Entities;
using Billing.Domain.Enums;
using Billing.Domain.Proration;

namespace Billing.Tests.Domain;

public class ProrationCalculatorTests
{
    private static Plan Plan(decimal monthly, decimal annual) => new() { MonthlyPrice = monthly, AnnualPrice = annual };

    [Fact]
    public void Upgrade_MidCycle_PositiveNet()
    {
        var oldP = Plan(10m, 100m);
        var newP = Plan(20m, 200m);
        var start = new DateTime(2026, 1, 1);
        var end = new DateTime(2026, 2, 1); // 31 days
        var change = new DateTime(2026, 1, 16); // midpoint-ish
        var net = ProrationCalculator.Compute(oldP, newP, BillingCycle.Monthly, start, end, change);
        Assert.True(net > 0);
    }

    [Fact]
    public void Downgrade_MidCycle_NegativeNet()
    {
        var oldP = Plan(20m, 200m);
        var newP = Plan(10m, 100m);
        var start = new DateTime(2026, 1, 1);
        var end = new DateTime(2026, 2, 1);
        var change = new DateTime(2026, 1, 16);
        var net = ProrationCalculator.Compute(oldP, newP, BillingCycle.Monthly, start, end, change);
        Assert.True(net < 0);
    }

    [Fact]
    public void ChangeAtPeriodEnd_ReturnsZero()
    {
        var oldP = Plan(10m, 100m);
        var newP = Plan(20m, 200m);
        var start = new DateTime(2026, 1, 1);
        var end = new DateTime(2026, 2, 1);
        var net = ProrationCalculator.Compute(oldP, newP, BillingCycle.Monthly, start, end, end);
        Assert.Equal(0m, net);
    }

    [Fact]
    public void Rounding_HalfUp()
    {
        // force a value that needs rounding
        var oldP = Plan(10m, 100m);
        var newP = Plan(10.005m, 100m); // difference tiny
        var start = new DateTime(2026, 1, 1);
        var end = new DateTime(2026, 1, 11); // 10 days
        var change = new DateTime(2026, 1, 6); // 5 days remainder
        var net = ProrationCalculator.Compute(oldP, newP, BillingCycle.Monthly, start, end, change);
        Assert.Equal(0.00m, Math.Round(net, 2));
    }

    [Fact]
    public void Precise_Upgrade_HalfPeriod()
    {
        var oldP = Plan(100m, 1000m);
        var newP = Plan(200m, 2000m);
        var start = new DateTime(2026, 1, 1);
        var end = new DateTime(2026, 1, 31); // 30 days
        var change = new DateTime(2026, 1, 16); // 15 days remainder
        var net = ProrationCalculator.Compute(oldP, newP, BillingCycle.Monthly, start, end, change);
        Assert.Equal(50.00m, net);
    }

    [Fact]
    public void Precise_Downgrade_HalfPeriod()
    {
        var oldP = Plan(200m, 2000m);
        var newP = Plan(100m, 1000m);
        var start = new DateTime(2026, 1, 1);
        var end = new DateTime(2026, 1, 31);
        var change = new DateTime(2026, 1, 16);
        var net = ProrationCalculator.Compute(oldP, newP, BillingCycle.Monthly, start, end, change);
        Assert.Equal(-50.00m, net);
    }

    [Fact]
    public void Precise_Rounding_To2Decimals()
    {
        var oldP = Plan(10m, 100m);
        var newP = Plan(20m, 200m);
        var start = new DateTime(2026, 1, 1);
        var end = new DateTime(2026, 1, 4); // 3 days
        var change = new DateTime(2026, 1, 2); // 2 days remainder
        var net = ProrationCalculator.Compute(oldP, newP, BillingCycle.Monthly, start, end, change);
        Assert.Equal(6.67m, net);
    }

    [Fact]
    public void Precise_FullPeriodDifference()
    {
        var oldP = Plan(10m, 100m);
        var newP = Plan(25m, 250m);
        var start = new DateTime(2026, 1, 1);
        var end = new DateTime(2026, 2, 1); // 31 days
        var change = new DateTime(2026, 1, 1); // full remainder
        var net = ProrationCalculator.Compute(oldP, newP, BillingCycle.Monthly, start, end, change);
        Assert.Equal(15.00m, net);
    }

    [Fact]
    public void AnnualCycle_Works()
    {
        var oldP = Plan(10m, 100m);
        var newP = Plan(10m, 120m);
        var start = new DateTime(2026, 1, 1);
        var end = new DateTime(2027, 1, 1);
        var change = new DateTime(2026, 7, 1);
        var net = ProrationCalculator.Compute(oldP, newP, BillingCycle.Annual, start, end, change);
        Assert.True(net > 0);
    }
}