# SoftDrink

Multi-tenant billing and subscription SaaS platform ("fin-brain"). Sold to companies as tenants; the system manages companies and their subscriptions, including trials, plan changes, renewals, dunning, proration, and webhook-driven payment events.

## Tech Stack

- ASP.NET Core 10 (controllers)
- EF Core + SQL Server
- xUnit (unit + integration via Testcontainers)
- MediatR (in-process notifications)
- Clean Architecture (one project per layer)

## Architecture

Clean Architecture with four projects plus tests:

- `Billing.Domain` — entities, enums, value objects, state machine rules, domain events
- `Billing.Application` — use cases, DTOs, validators, MediatR handlers, abstractions (repositories, Unit of Work, `Result`/`ResultBuilder`)
- `Billing.Infrastructure` — EF Core DbContext, repositories, UoW implementation, hosted background services, webhook signature verification, dummy payment provider, logging
- `Billing.Api` — controllers, middleware, JWT auth, tenant resolution, Swagger, DI wiring
- `Billing.Tests` — xUnit: unit + integration

## Multi-Tenancy

- Shared database with a `TenantId` discriminator and EF Core global query filters.
- Tenant resolved from JWT claim; `ITenantContext` injected into handlers.
- Tenant isolation is mandatory on every query.

## Core Entities

- **Tenant** — Id, Name, CreatedAt
- **Plan** — Id, Name (Starter/Pro/Enterprise), MonthlyPrice, AnnualPrice, Features
- **Subscription** — Id, TenantId, PlanId, Status (state machine), BillingCycle (Monthly/Annual), CurrentPeriodStart, CurrentPeriodEnd, TrialEndsAt, GracePeriodEndsAt, CancelAtPeriodEnd, RowVersion (optimistic concurrency)
- **Invoice** — Id, TenantId, SubscriptionId, Amount, Currency (USD), Status (Draft/Issued/Paid/Failed/Void), PeriodStart/End, IssuedAt, PaidAt, ProrationDetails
- **PaymentEvent** — Id, ProviderEventId (unique), EventType, ProcessedAt, Payload (idempotency log)

## Subscription State Machine

- `Trial → Active` (successful payment) | `Trial → Expired` (no payment after 30 days)
- `Active → PastDue` (payment fails) → `Suspended` (after 3-day grace) → `Cancelled` (after 30 days)
- `Active → Cancelled` (immediate or end-of-period)

## Key Flows

- **Trial**: 30 days free on signup; no payment method required upfront.
- **Renewal**: Background service checks subscriptions nearing period end, creates invoice, charges.
- **Dunning**: on `payment_intent.failed` webhook → PastDue + 3-day grace; background service suspends/cancels when grace expires.
- **Proration**: on mid-cycle plan change, computes unused credit on old plan + charge for new plan remainder; round half-up to 2 decimals; single invoice line item.
- **Webhooks**: Stripe-like HMAC signature verification, idempotency via `PaymentEvent` (unique event id), processed transactionally.

## Money & Concurrency

- Single currency USD, amounts stored as `decimal(18,2)`.
- `RowVersion` (SQL Server `rowversion`) as EF concurrency token on Subscription.
- Proration rounding: half-up to 2 decimals.

## Running Tests

```bash
dotnet test
```
