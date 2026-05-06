# 🚀 TransitInsight

A full-stack ASP.NET Core MVC application for managing public transport stop places and real-time departures using the Entur API.

---

## 📌 Overview

TransitInsight is a web-based dashboard that allows users to:

- Search and save public transport stops from Entur
- Import and display real-time departures
- View delays and statistics
- Explore nearby stops using GPS
- Monitor transport data through a modern dashboard

The system is designed with a clean architecture and focuses on real-time data integration.

---

## 🧱 Tech Stack

- ASP.NET Core MVC (.NET 10)
- Entity Framework Core (SQLite)
- Entur API (Geocoder + Journey Planner)
- JavaScript (Fetch API)
- Leaflet.js (Maps)
- Chart.js (Dashboard charts)
- ASP.NET Identity (Authentication)

---

## ⚙️ Features

### 🔍 Search & Save Stops
- Search stop places using Entur API
- Save stops to local database
- Avoid duplicates automatically

### 🚌 Live Departures
- Import real-time departures from Entur
- Display:
  - Line
  - Destination
  - Expected time
  - Delay status

### 📊 Dashboard
- Total stop places
- Total departures
- Delayed departures
- Average delay
- Chart visualization (transport modes)
- Latest departures list

### 📍 Nearby Stops (GPS)
- Detect user location
- Fetch nearby stops
- Display on map with markers
- View live departures per stop

### 🔐 Authentication
- User registration and login
- Protected pages using `[Authorize]`

---

## 🧠 Architecture

- Controllers → Handle logic and API calls
- Services → EnturService for external API
- Models → StopPlace, Departure, ViewModels
- Views → Razor pages with dynamic rendering
- Database → SQLite via EF Core

---

## 🔄 Real-Time Behavior

- Departures are fetched dynamically from Entur API
- Data is refreshed periodically (auto-refresh)
- Dashboard updates based on latest data
- Nearby stops use live GPS (when available)

---

## 🧪 Testing & Environment

### 🖥️ Desktop (Stationary PC)
- Application runs correctly
- GPS often unavailable or inaccurate
- Falls back to IP-based location (low precision)

### 💻 Laptop Testing
- Tested on laptop environment
- Result: Same behavior as desktop
- GPS still limited due to browser/device constraints

### 📱 Mobile (Recommended)
- Provides accurate GPS coordinates
- Best environment for testing Nearby Stops
- Fully demonstrates real-time location features

---

## ⚠️ GPS & Browser Limitations

Modern browsers enforce strict security rules:

- GPS requires:
  - `https://` OR
  - `localhost`

- Using:

http://192.168.x.x

may block location access

### Conclusion:
- GPS reliability depends on:
- Device (mobile vs desktop)
- Browser security
- Network context

---

## 🧩 Challenges

- Handling browser GPS restrictions
- Mapping Entur API data to internal models
- Ensuring consistent real-time updates
- Avoiding duplicate data in database
- Managing async API calls and UI updates

---

## 💡 Improvements (Future Work)

- SignalR for true real-time updates (no refresh)
- Background service for automatic departure sync
- Favorites system for stops
- Better error handling & logging
- Deployment to cloud (Azure)

---

## 🎯 Reflection

The project demonstrates:

- Integration with external APIs
- Full-stack development using ASP.NET Core MVC
- Database design and data flow
- Real-time data handling concepts
- Awareness of browser and device limitations

Special attention was given to GPS functionality and cross-device testing, highlighting differences between desktop and mobile environments.

---

## 🏁 Conclusion

TransitInsight is a complete and functional system that simulates a real-world transport dashboard.

It combines:
- Backend logic
- API integration
- Frontend visualization
- Real-time data concepts

---

## 👨‍💻 Author

Developed as part of exam/project work.

---