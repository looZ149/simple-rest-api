# SampleTracker

A small REST API for logging malware sample metadata (hashes, YARA matches, analysis notes) from a RE lab.

## Stack

- ASP.NET Core Minimal API (.NET 10)
- In-memory list for now — Postgres via EF Core planned

## Run

```bash
cd SampleTracker
dotnet run
```

## Endpoints

| Method | Route              | Description          |
|--------|--------------------|-----------------------|
| GET    | `/samples`         | List all samples      |
| GET    | `/samples/{id}`    | Get one sample by id  |
| POST   | `/samples`         | Add a sample           |
| DELETE | `/samples/{id}`    | Remove a sample by id |

Example request bodies are in `SampleTracker/SampleTracker.http`.

## Status

CRUD against an in-memory list. Next up: input validation, then swapping the list for Postgres.
