namespace Billing.Domain.Enums;

public enum SubscriptionStatus
{
    Trial,
    Active,
    PastDue,
    Suspended,
    Cancelled,
    Expired
}