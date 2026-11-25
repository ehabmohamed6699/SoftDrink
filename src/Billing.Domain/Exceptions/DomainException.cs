namespace Billing.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}

public class InvalidStateTransitionException : DomainException
{
    public InvalidStateTransitionException(Enums.SubscriptionStatus from, Enums.SubscriptionStatus to)
        : base($"Invalid subscription state transition: {from} -> {to}.") { }
}