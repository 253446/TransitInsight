# 🚆 TransitInsight

## 📌 Overview
TransitInsight is a full-stack ASP.NET Core MVC web application for managing public transport data using the Entur API.

The application allows users to manage stop places, view departures, analyze delays, and integrate real-time transport data.

---

## 🛠 Technologies
- ASP.NET Core MVC (.NET 10)
- Entity Framework Core
- SQLite
- Chart.js
- Bootstrap
- ASP.NET Identity
- Entur API (GraphQL + REST)

---

## 🧱 Architecture
The project follows the MVC pattern:

- **Models:** StopPlace, Departure, FavoriteStop, ImportLog
- **Views:** Razor Views for UI
- **Controllers:** Handle CRUD and API logic
- **Services:** EnturService for API integration
- **Data:** ApplicationDbContext with EF Core

---

## 🗄 Database
SQLite database with the following entities:

- StopPlace (1)
- Departure (many)
- FavoriteStop
- ImportLog

Relationship:
- One StopPlace → Many Departures

---

## 🔄 Features

### CRUD
- Create, Read, Update, Delete StopPlaces and Departures

### API Integration
- Search stops using Entur API
- Import real-time departures
- Nearby stops using geolocation

### Dashboard
- Statistics (total stops, departures, delays)
- Chart.js visualization
- Latest departures list

### Authentication
- ASP.NET Core Identity
- Register/Login functionality
- Admin pages protected using `[Authorize]`

### Unit Tests
- Delay calculation test
- Database CRUD test using InMemory database

---

## 🔐 Login Info
Users can register using:


/Identity/Account/Register


Then login via:


/Identity/Account/Login


---

## ▶️ How to Run

```bash
dotnet restore
dotnet build
dotnet ef database update
dotnet run
🧪 Run Tests
dotnet test
📊 Git Workflow
main branch
dev branch
feature commits
clean commit messages
🎯 Conclusion

This project demonstrates a complete full-stack web application using ASP.NET Core MVC with database, API integration, authentication, and testing.

🚀 Future Improvements
Role-based authorization (Admin/User)
More advanced analytics
UI enhancements
Deployment to cloud