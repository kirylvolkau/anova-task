# Anova test task

This repository contains solution to the Anova recrutation test task:

- [ ] implementation of devices controller
  - [ ] `POST /device` (create) device
  - [ ] `GET /device/{device_id}` (get by id) device
  - [ ] `PUT /device/{device_id}` (update by id and new value) device
  - [ ] `GET /device` (get all) devices
  - [ ] `DELETE /device/{device_id}` (delete by id) device
- [ ] implementatino of readings controller
  - [ ] `POST /readings` (save) list of readings
  - [ ] `GET /readings/{device_id}/{from_timestamp}/{to_timestamp}` (get list of readings) based on the device ID and time window
- [ ] documentation of the code
- [ ] docker-compose for database and the code
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

## Implementation details

- just because I'm more used to it the `Dapper` was used for execution of SQL queries
- error handling was kept as simple as possible: just returning `null` in edge cases and parsing it to logic in code (not depending on types for errors)
- `type-per-file` pattern was ommited - for instance `Models.cs` contains two classes
- `RESTful` was followed not completely: for instance, `DELETE` operation should be idempotent, and it is not (I believe the alternative is called `HATEOAS`)
- mapping between layers (i.e. presentation and data access layer) has been skipped for the simplicity purposes - in real life I would use at least something like `Automapper`
- handling of insertion of `records` could be improved: the error could be specifc about which device was not found (currently basing on exception from `Dapper`)
- smaller details have been added to the code documentation
- tests have not been implemented (see email) - however, I would use `xUnit` with `Moq` and `FluentValidations` for unit tests and combination of `Specflow` and `FluentDocker` for integration tests
