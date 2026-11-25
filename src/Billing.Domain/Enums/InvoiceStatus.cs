namespace Billing.Domain.Enums;

public enum InvoiceStatus
{
    Draft,
    Issued,
    Paid,
    Failed,
    Void
}