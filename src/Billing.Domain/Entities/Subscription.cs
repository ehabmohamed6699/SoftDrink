using Billing.Domain.Enums;
using Billing.Domain.Exceptions;
using Billing.Domain.StateMachines;

namespace Billing.Domain.Entities;

public class Subscription
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public Guid PlanId { get; set; }
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Trial;
    public BillingCycle BillingCycle { get; set; } = BillingCycle.Monthly;
    public DateTime CurrentPeriodStart { get; set; }
    public DateTime CurrentPeriodEnd { get; set; }
    public DateTime? TrialEndsAt { get; set; }
    public DateTime? GracePeriodEndsAt { get; set; }
    public bool CancelAtPeriodEnd { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public void TransitionTo(SubscriptionStatus target)
    {
        if (!SubscriptionStateMachine.CanTransition(Status, target))
            throw new InvalidStateTransitionException(Status, target);
        Status = target;
    }

    public void RequestCancellation(bool immediate)
    {
        if (immediate)
        {
            TransitionTo(SubscriptionStatus.Cancelled);
            CancelAtPeriodEnd = false;
        }
        else
        {
            if (Status != SubscriptionStatus.Active && Status != SubscriptionStatus.Trial)
                throw new InvalidStateTransitionException(Status, SubscriptionStatus.Cancelled);
            CancelAtPeriodEnd = true;
        }
    }
}