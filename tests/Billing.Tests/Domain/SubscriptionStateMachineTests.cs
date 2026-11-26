using Billing.Domain.Entities;
using Billing.Domain.Enums;
using Billing.Domain.Exceptions;
using Billing.Domain.StateMachines;

namespace Billing.Tests.Domain;

public class SubscriptionStateMachineTests
{
    [Theory]
    [InlineData(SubscriptionStatus.Trial, SubscriptionStatus.Active)]
    [InlineData(SubscriptionStatus.Trial, SubscriptionStatus.Expired)]
    [InlineData(SubscriptionStatus.Active, SubscriptionStatus.PastDue)]
    [InlineData(SubscriptionStatus.Active, SubscriptionStatus.Cancelled)]
    [InlineData(SubscriptionStatus.PastDue, SubscriptionStatus.Active)]
    [InlineData(SubscriptionStatus.PastDue, SubscriptionStatus.Suspended)]
    [InlineData(SubscriptionStatus.Suspended, SubscriptionStatus.Cancelled)]
    [InlineData(SubscriptionStatus.Suspended, SubscriptionStatus.Active)]
    public void CanTransition_Valid_ReturnsTrue(SubscriptionStatus from, SubscriptionStatus to)
    {
        Assert.True(SubscriptionStateMachine.CanTransition(from, to));
    }

    [Theory]
    [InlineData(SubscriptionStatus.Expired, SubscriptionStatus.Active)]
    [InlineData(SubscriptionStatus.Cancelled, SubscriptionStatus.Active)]
    [InlineData(SubscriptionStatus.Trial, SubscriptionStatus.PastDue)]
    [InlineData(SubscriptionStatus.Expired, SubscriptionStatus.Trial)]
    public void CanTransition_Invalid_ReturnsFalse(SubscriptionStatus from, SubscriptionStatus to)
    {
        Assert.False(SubscriptionStateMachine.CanTransition(from, to));
    }

    [Fact]
    public void TransitionTo_Valid_Works()
    {
        var sub = new Subscription { Status = SubscriptionStatus.Trial };
        sub.TransitionTo(SubscriptionStatus.Active);
        Assert.Equal(SubscriptionStatus.Active, sub.Status);
    }

    [Fact]
    public void TransitionTo_Invalid_Throws()
    {
        var sub = new Subscription { Status = SubscriptionStatus.Expired };
        Assert.Throws<InvalidStateTransitionException>(() => sub.TransitionTo(SubscriptionStatus.Active));
    }

    [Fact]
    public void RequestCancellation_Immediate_CancelsNow()
    {
        var sub = new Subscription { Status = SubscriptionStatus.Active };
        sub.RequestCancellation(true);
        Assert.Equal(SubscriptionStatus.Cancelled, sub.Status);
        Assert.False(sub.CancelAtPeriodEnd);
    }

    [Fact]
    public void RequestCancellation_AtPeriodEnd_SetsFlag()
    {
        var sub = new Subscription { Status = SubscriptionStatus.Active };
        sub.RequestCancellation(false);
        Assert.Equal(SubscriptionStatus.Active, sub.Status);
        Assert.True(sub.CancelAtPeriodEnd);
    }
}