# 🏈 Fantasy Sports Team Management System

This project implements a cloud-deployed **Fantasy Sports Team Management System** using a **Microservices Architecture**, hosted on **Azure App Service** with a centralized **Azure SQL Database**. It is built for DEV422 at Bellevue College.

## 🎯 Objective

Design and deploy a scalable system composed of three independent microservices:
- **Team Management Service**
- **Player Management Service**
- **Performance Tracking Service**

Each microservice handles its own responsibilities, communicates via REST APIs, and connects to a dedicated table in the centralized Azure SQL database.

---

## 🧱 Architecture Overview

![Architecture Diagram](./assets/architecture-diagram.png) <!-- Replace with your actual diagram if available -->

- **UI Layer**: Web interface for user interaction (e.g., index.html).
- **REST APIs**: All services expose and consume RESTful endpoints.
- **Azure SQL Database**: Each service interacts with its own tables in a centralized DB.
- **Service Isolation**: Services do not directly access each other’s databases — communication is done via APIs only.

---

## 🔧 Microservices

### 1. Team Management Service

📍 **URL**: `https://fantasysportsteammanagementsystem.azurewebsites.net/api/team`

| Method | Endpoint             | Description               |
|--------|----------------------|---------------------------|
| POST   | `/create`            | Create a new team         |
| GET    | `/`                  | List all teams            |
| PUT    | `/update/{id}`       | Update team name          |
| GET    | `/{teamId}/roster`   | Fetch team roster         |

---

### 2. Player Management Service

📍 **URL**: `https://player-management-service.azurewebsites.net/api/player`

| Method | Endpoint             | Description               |
|--------|----------------------|---------------------------|
| GET    | `/getTeamRoaster`    | Get all players in a team |
| POST   | `/draft`             | Draft player to team      |
| POST   | `/release`           | Release player from team  |

---

### 3. Performance Tracking Service

📍 **URL**: `https://performance-tracking-service.azurewebsites.net/api/competitions`

| Method | Endpoint         | Description                         |
|--------|------------------|-------------------------------------|
| POST   | `/simulate`      | Simulate game, update stats         |
| GET    | `/`              | Get all performance stats           |
| PUT    | `/{id}`          | Update specific stat record         |

---

## 🗃️ Database Schema

- `Teams`: Managed by Team Service  
- `Players`: Managed by Player Service  
- `PerformanceStats`: Managed by Performance Service  

All services use **Azure SQL Database**, but only interact with their own tables.

---

## 🔁 Inter-Service Communication

- **Team Service** fetches player data from **Player Service**
- **Performance Service** fetches team and player data from both services
- Services communicate **only via APIs**, not direct DB access

---

## 🔒 Security

- Azure SQL secured via firewall rules and app settings
- Sensitive configs managed via Azure App Service environment variables
- (Optional) Token-based authentication can be added for production

---

## 🧪 How to Test

### 🔹 Team Service
- Create team: `POST /api/team/create`
- Get all teams: `GET /api/team`
- Update team: `PUT /api/team/update/{id}`
- Get team roster: `GET /api/team/{teamId}/roster`

### 🔹 Player Service
- Draft player: `POST /api/player/draft`
- Release player: `POST /api/player/release`
- Get team roster: `GET /api/player/getTeamRoaster?teamId={id}`

### 🔹 Performance Service
- Simulate competition: `POST /api/competitions/simulate`
- Get stats: `GET /api/competitions`
- Update stat: `PUT /api/competitions/{id}`

---

## 🌐 Live URLs

| Service | URL |
|--------|-----|
| Team Management | `https://fantasysportsteammanagementsystem.azurewebsites.net` |
| Player Management | `https://player-management-service.azurewebsites.net` |
| Performance Tracking | `https://performance-tracking-service.azurewebsites.net` |

---
