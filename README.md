# Anova test task

This repository contains solution to the Anova recrutation test task:

- [X] implementation of devices controller
  - [X] `POST /device` (create) device
  - [X] `GET /device/{device_id}` (get by id) device
  - [X] `PUT /device/{device_id}` (update by id and new value) device
  - [X] `GET /device` (get all) devices
  - [X] `DELETE /device/{device_id}` (delete by id) device
- [X] implementatino of readings controller
  - [X] `POST /readings` (save) list of readings
  - [X] `GET /readings/{device_id}/{from_timestamp}/{to_timestamp}` (get list of readings) based on the device ID and time window
- [X] documentation of the code
- [X] docker-compose for database and the backend
- [ ] unit tests
  - [ ] readings storage tests
  - [ ] device storage tests
- [ ] integration tests
  - [ ] device controller tests
  - [ ] readings controller tests

## How to run

You would need to have `docker` and `docker compose` installed on your local machine. After that, run

```shell
docker compose up
```

from the root of the repo. Go to:

- `http://localhost:8080/swagger/` if your OS is MacOS
- `http://127.0.0.1:8080/swagger/` if your OS is Unix
- `http://0.0.0.1:8080/swagger/` if you are using Windows

## Assumptions

- timestamp is in unix seconds
- timestamp will be provided by users in unix seconds
- `POST /readings` endpoint can accept readings from multiple devices
- `reading_type` type is `string` since we don't know all the restrictions / requirements and we don't want to lost the data - so we choose the least restrictive one. in real-life system it would probably be some kind of enum
- same goes for the `raw_value`: we can have different reading types - maybe, some of them reports reading as a string - we don't want to lose it

## Implementation details

- just because I'm more used to it the `Dapper` was used for execution of SQL queries
- error handling was kept as simple as possible: just returning `null` in edge cases and parsing it to logic in code (not depending on types for errors)
- `type-per-file` pattern was ommited - for instance `Models.cs` contains two classes
- `RESTful` was followed not completely: for instance, `DELETE` operation should be idempotent, and it is not (I believe the alternative is called `HATEOAS`)
- mapping between layers (i.e. presentation and data access layer) has been skipped for the simplicity purposes - in real life I would use at least something like `Automapper`
- handling of insertion of `records` could be improved: the error could be specifc about which device was not found (currently basing on exception from `Dapper`)
- smaller details have been added to the code documentation
- tests have not been implemented (see email) - however, I would use `xUnit` with `Moq` and `FluentValidations` for unit tests and combination of `Specflow` and `FluentDocker` for integration tests
- `dotnet format` has been used for formatting (no `.editorconfig`)
