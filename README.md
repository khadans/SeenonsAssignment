## About the project
This web api returns waste streams information with availability for pickup on schedule.

## Built With

- Language: C#
- Framework: .Net Core 3.1
- IDE: Visual Studio 2022
- Storage: Postgresql

## Architecture

### Overall Architecture

#### Solution contains below projects:

- `Seenons.WebApi` is responsible for all endpoints
- `Seenons` is domain project that contains ports, use cases, models etc
- `Seenons.Persistence` is responsible for the interaction with the database
- All above projects have their own test projects with unit/integration tests

## Getting started

### Installation

Run docker command from the root folder:

`docker-compose up` 

It will run web api, postgresql database and PGAdmin tool in docker containers.
Database (`seenonsDb`) is populated with test data from seed.sql (see root folder)
- API: http://localhost:5000
- PGAdmin: http://localhost:5050/browser/

To connect to the PGAdmin use email: `pgadmin4@pgadmin.org` and password `admin1234`.

To register a server in PGAdmin use host `postgresql_database`, usename `admin`, password `admin1234`.

## To Interact with the API
To test endpoints you can use Swagger UI: http://localhost:5000/swagger/index.html

Request example: http://localhost:5000/api/wastestreams?PostalCode=1000&Weekdays=1&Weekdays=2

## Future scope
- Adding authentication and authorization
- Adding resilicence policy for database access
- Adding correlation headers/request response logging for easier tracing of the issues.
