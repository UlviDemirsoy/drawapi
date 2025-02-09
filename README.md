# DrawAPI - Tournament Group Draw System

DrawAPI is a **.NET 8 Web API** designed to manage **randomized group draws for tournaments**. The system ensures **fair team distribution** by enforcing rules that prevent teams from the same country from being placed in the same group (depending on the group size).

## Features
- **Randomized group assignment** for teams.
- **Enforces country diversity per group** (different rules for 4-group vs. 8-group draws).
- **REST API with endpoints for draws, groups, and teams**.
- **Unit-tested with xUnit and Moq**.
- **Uses PostgreSQL as DB**.

---

## 🚀 Setup & Run the Project

### Start PostgreSQL Database with Docker
This project uses **PostgreSQL** as the database. To start a PostgreSQL container, run:
```sh
docker run --name multiverse -p 5477:5432 -e POSTGRES_DB=c137 -e POSTGRES_USER=rick -e POSTGRES_PASSWORD=plumbus -d postgres
dotnet ef migrations add InitialMigration
dotnet ef database update

# API Endpoints

This API provides **RESTful endpoints** to manage **draws, groups, and teams**.

## Draws API (Manage Tournament Draws)

### Create a New Draw
**`POST /api/draws`**
### List all Draws
**`GET /api/draws/list`**
### Get Draw by Id
**`GET /api/draws/{id}`**

## Groups API (Manage Tournament Groups)

### List all Groups
**`GET /api/groups/list`**
### Get Group by Id
**`GET /api/groups/{id}`**

## Teams API (Manage Teams)

### List all Teams
**`GET /api/teams/list`**



