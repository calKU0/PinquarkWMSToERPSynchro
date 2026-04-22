# Pinquark WMS to ERP Synchronization Service

`PinquarkWMSToERPSynchro` is a .NET Worker Service that continuously synchronizes data from Pinquark WMS integration endpoints into SQL Server (`wms` schema) for ERP-side processing and reporting.

## What this service does

The worker runs endpoint sync jobs in the background and, for each job:

1. Calls a Pinquark API endpoint via typed `HttpClient`.
2. Maps response payloads to internal models.
3. Persists data using bulk upsert stored procedures (`MERGE` + TVP).

Supported sync domains:

- Zones
- Locations
- Warehouse stock
- Documents
- Tasks
- Logistic units (and LU content)
- Operations
- Custom operations

## Solution layout

- **PinquarkWMSToERPSynchro.Service**  
  Worker host, scheduling loops, dependency orchestration, DI setup, and logging bootstrap.

- **PinquarkWMSToERPSynchro.Infrastructure**  
  API client, Dapper DB executor, repositories, and mapping logic.

- **PinquarkWMSToERPSynchro.Contracts**  
  Shared DTOs, models, interfaces, and settings contracts.

- **PinquarkWMSToErpSynchro.Database**  
  SQL scripts for upsert procedures.

## Runtime behavior

- The worker starts independent periodic sync loops for configured endpoints.
- Endpoints can define dependencies (`DependsOn`) to enforce foreign-key-safe order.
- Dependencies are resolved recursively before syncing the dependent endpoint.
- Circular dependency chains are detected and logged as warnings.

## Reliability and error handling

HTTP requests use Polly retry handling (typed `HttpClient` policy):

- Retries transient HTTP/network failures
- Exponential backoff
- Up to 3 retries before failure is surfaced

Synchronization errors are logged per endpoint and do not stop other endpoint loops.

## Logging and observability

Logging is implemented with Serilog and includes:

- Console output
- Rolling file logs
- Optional Seq sink
- Optional email sink

Log level overrides reduce infrastructure noise while keeping sync-level operational visibility.

## Data layer approach

Persistence is designed for recurring batch synchronization:

- Dapper for execution
- Table-valued parameters for bulk payload transfer
- Stored procedures with idempotent upsert semantics

This keeps sync runs predictable and efficient for larger payloads.

## Database scripts

The database project includes scripts used by the service, including:

- Bulk upsert procedures for each domain

## Design goals

- Clear separation of responsibilities across projects
- Stable long-running worker behavior
- FK-safe synchronization order via endpoint dependencies
- Efficient bulk persistence for high-frequency sync jobs

## License

This project is **proprietary and confidential**.

It was developed for a client and is **not permitted to be shared, redistributed, or used** without explicit written permission from the owner.

See [LICENSE](LICENSE) for details.

---

© 2026-present [calKU0](https://github.com/calKU0)
