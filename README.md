# TransitInsight

A full-stack ASP.NET Core MVC transport dashboard using Entur API, Entity Framework Core, SQLite and real-time departure analytics.

---

# Project Overview

TransitInsight is a modern transport management and analytics system developed with ASP.NET Core MVC.

The application integrates with the Entur public transport API to provide real-time departure information, stop place search, live dashboard analytics and transport statistics.

The system demonstrates:

- ASP.NET Core MVC architecture
- Entity Framework Core with SQLite
- REST API integration
- Authentication and authorization
- CRUD operations
- Real-time dashboard updates
- Chart.js analytics
- Unit testing with xUnit
- GitFlow workflow

---

# Features

## Dashboard Analytics

- Live transport dashboard
- Departure statistics
- Delay analytics
- Average delay calculation
- Most active stop place
- Next departure information
- Automatic live refresh every 60 seconds
- Chart.js visualizations

## Stop Place Management

- Create stop places
- Edit stop places
- Delete stop places
- View stop details
- Store Entur stop IDs
- Store GPS coordinates

## Live Departures

- Real-time departures from Entur API
- Delay calculations
- Transport mode tracking
- Destination tracking
- Automatic refresh system

## Search Entur API

- Search stop places directly from Entur
- Save stop places to local database
- GPS/location support
- Nearby stops functionality

## Authentication

- ASP.NET Core Identity
- Register/Login system
- Protected routes with Authorize attribute

## Testing

- Unit testing with xUnit
- Dashboard tests
- StopPlace tests
- Departure tests

---

# Technologies Used

- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQLite
- ASP.NET Core Identity
- Bootstrap 5
- Chart.js
- Entur API
- xUnit
- Git & GitHub

---

# Architecture

The project follows the MVC architecture pattern:

```txt
Controllers/
Models/
Views/
Services/
Data/
```

Main components:

- Controllers handle HTTP requests
- Models represent domain entities
- Views render Razor UI pages
- Services handle API integration
- DbContext manages database access

---

# Database Models

## StopPlace

Represents transport stop locations.

## Departure

Represents departures connected to stop places.

## ImportLog

Stores API import history and refresh tracking.

## FavoriteStop

Stores user-selected favorite stops.

---

# Relationships

```txt
StopPlace (1) ---> (Many) Departures
```

---

# Entur API Integration

The application integrates with:

- Entur Geocoder API
- Entur Journey Planner API

Used for:

- Stop place search
- Live departures
- Nearby stops
- Real-time transport information

---

## Screenshots

### Dashboard
![Dashboard](screenshots/dashboard.png)

---

### Departures
![Departures](screenshots/departure.png)

---

### Stop Places
![Stop Places](screenshots/stopplaces.png)

---

### Search Entur
![Search Entur](screenshots/search-entur.png)

---

### Login System
![Login](screenshots/login.png)

# Installation

## Clone Repository

```bash
git clone <repository-url>
```

---

## Navigate to Project

```bash
cd TransitInsight
```

---

## Restore Packages

```bash
dotnet restore
```

---

## Apply Database Migrations

```bash
dotnet ef database update
```

---

## Run Application

```bash
dotnet run
```

---

# Testing

Run all tests:

```bash
dotnet test
```

Current test coverage includes:

- Dashboard logic
- StopPlace model tests
- Departure model tests

---

# Git Workflow

The project uses GitFlow-inspired branching:

```txt
main
dev
extraFeature
```

Used for:

- Feature development
- Testing
- Safe merges
- Version control

---

# Real-Time Dashboard

The dashboard automatically refreshes departure data every 60 seconds using the Entur API.

Features include:

- Live departure updates
- Delay monitoring
- Transport statistics
- Dynamic charts

---

# GPS and Nearby Stops

The application supports:

- Browser geolocation
- Nearby stop search
- Manual place search

Testing showed that GPS accuracy depends on the device:

- Mobile devices provide more accurate GPS
- Desktop/laptop browsers often use IP-based location

Therefore, the system supports both automatic GPS and manual location search.

---

# Security

The application uses:

- ASP.NET Core Identity
- Authentication
- Authorization
- Anti-forgery tokens
- Protected controller actions

---

# Reflection

This project demonstrates how a modern transport dashboard can be developed using ASP.NET Core MVC and real-time API integration.

Key learning outcomes:

- MVC architecture
- Database design
- REST API integration
- Authentication systems
- Frontend analytics
- Real-time data refresh
- Git workflow
- Unit testing

---

# Author

Reza Gohari

---

# License

This project was developed for educational purposes.