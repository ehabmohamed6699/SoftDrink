using Billing.Domain.Enums;

namespace Billing.Domain.StateMachines;

public static class SubscriptionStateMachine
{
    private static readonly HashSet<(SubscriptionStatus From, SubscriptionStatus To)> Allowed = new()
    {
        (SubscriptionStatus.Trial, SubscriptionStatus.Active),
        (SubscriptionStatus.Trial, SubscriptionStatus.Expired),
        (SubscriptionStatus.Active, SubscriptionStatus.PastDue),
        (SubscriptionStatus.Active, SubscriptionStatus.Cancelled),
        (SubscriptionStatus.PastDue, SubscriptionStatus.Active),
        (SubscriptionStatus.PastDue, SubscriptionStatus.Suspended),
        (SubscriptionStatus.Suspended, SubscriptionStatus.Cancelled),
        (SubscriptionStatus.Suspended, SubscriptionStatus.Active)
    };

    public static bool CanTransition(SubscriptionStatus from, SubscriptionStatus to)
        => Allowed.Contains((from, to));
}