# Koos Matli Primary School System

## 📚 Project Overview

The **Koos Matli Primary School System** is a digital platform developed to improve communication, accessibility, and information management for **Koos Matli Primary School**.

The system currently consists of a **responsive public-facing school website** supported by a cloud-based REST API and Firebase Firestore database.

The solution was developed as part of our **Work Integrated Learning (WIL)** project and is designed around the school's need for a centralised digital platform where parents, learners, staff, and community members can easily access important school information.

For the final phase of the project, the system will be extended with a dedicated **Koos Matli Primary School mobile application developed using .NET MAUI**.

---

# 🎯 Project Objectives

The main objectives of the system are to:

* Provide the school with a professional online presence.
* Centralise important school information.
* Improve communication between the school and parents/guardians.
* Provide access to school announcements and news.
* Display staff information.
* Provide information about admissions.
* Allow users to contact the school electronically.
* Display upcoming school events and activities.
* Showcase school activities, achievements, and events through a gallery.
* Provide a scalable technical foundation for future mobile functionality.

---

# 🖥️ Current System

The current implementation is a **web-based school information system**.

The website provides the following pages:

### 🏠 Home

The Home page provides an introduction to Koos Matli Primary School and serves as the main entry point into the system.

It provides navigation to the major sections of the website and communicates the school's identity and purpose.

---

### ℹ️ About Us

The About Us page provides information about the school, including:

* School description
* School motto
* Mission
* Vision
* History
* Contact information
* Principal information

The information is retrieved through the API rather than being directly hard-coded into the website.

---

### 👩‍🏫 Our Staff

The Staff page displays information about school staff members.

Staff records include information such as:

* Full name
* Position
* Department
* Category
* Biography
* Email address
* Profile image
* Active status

Staff information is retrieved dynamically from the backend API.

---

### 📰 News & Announcements

The News and Announcements page allows users to view important school communication.

News records include:

* Title
* Content
* Publication date
* Category
* Image
* Published status

Only published news articles are displayed on the public website.

---

### 📅 School Calendar

The School Calendar provides users with access to upcoming school events.

Events include:

* Event title
* Description
* Date
* Time
* Location
* Category
* Publication status

Events are retrieved from the backend and ordered according to their dates.

This allows the school to maintain a centralised calendar rather than manually updating the website's page content.

---

### 🖼️ School Gallery

The Gallery allows the school to showcase activities, events, achievements, sports, culture, and academic activities.

Gallery items include:

* Title
* Description
* Image
* Category
* Date added
* Publication status

The gallery is also API-driven, allowing content to be managed independently from the website interface.

---

### 🎓 Admissions

The Admissions section provides parents and guardians with information about the school's admission process.

The system also supports the submission of admission applications.

Application information includes:

* Parent/guardian details
* Contact information
* Relationship to learner
* Learner details
* Date of birth
* Grade applying for
* Previous school
* Home address
* Additional information
* Application status
* Submission date

Submitted applications are stored through the backend API.

---

### 📩 Contact Us

The Contact page allows users to communicate directly with the school.

Users can submit:

* Name
* Email address
* Subject
* Message

The information is sent to the backend API and stored in Firebase Firestore.

---

# 🏗️ System Architecture

The current system follows a layered architecture:

```text
                    USER
                      │
                      ▼
          ┌──────────────────────┐
          │   ASP.NET Core MVC   │
          │       WEBSITE        │
          └──────────┬───────────┘
                     │
                     │ HTTP / JSON
                     ▼
          ┌──────────────────────┐
          │   Node.js + Express  │
          │      REST API        │
          └──────────┬───────────┘
                     │
                     │ Firebase Admin SDK
                     ▼
          ┌──────────────────────┐
          │  Firebase Firestore  │
          │       DATABASE       │
          └──────────────────────┘
```

### Architecture Flow

The system follows this flow:

**User → ASP.NET MVC Website → REST API → Firebase Firestore**

The website does **not** communicate directly with Firebase.

Instead, requests are sent to the REST API, which communicates with Firestore.

This provides a clear separation between:

* Presentation
* Application/API logic
* Data storage

This architecture also provides a foundation for future clients, including the planned mobile application.

---

# 🌐 Frontend — ASP.NET Core MVC

The public website is developed using:

* **ASP.NET Core MVC**
* **C#**
* **Razor Views**
* **HTML**
* **CSS**
* **JavaScript**

The MVC application is responsible for:

* Rendering web pages
* Handling navigation
* Communicating with the API
* Displaying API data
* Processing user-submitted forms
* Providing the user interface

The website communicates with the API through a dedicated:

```text
WebsiteApiService
```

This service centralises communication between the MVC application and the backend API.

---

# ⚙️ Backend — Node.js & Express

The backend API was implemented using:

* **Node.js**
* **Express.js**
* **Firebase Admin SDK**
* **CORS**
* **dotenv**

The API provides separate endpoints for different system resources.

Current endpoints include:

```text
/api/about
/api/staffmembers
/api/news
/api/events
/api/gallery
/api/admissions
/api/contact
/api/Applications
```

A health-check endpoint is also available:

```text
/health
```

The API follows a resource-based REST architecture.

For example:

```text
GET    /api/news
GET    /api/news/{id}

GET    /api/staffmembers
GET    /api/staffmembers/{id}
POST   /api/staffmembers
PUT    /api/staffmembers/{id}
DELETE /api/staffmembers/{id}
```

This makes the API predictable, reusable, and easier to maintain.

---

# 🔥 Database — Firebase Firestore

The system currently uses **Firebase Firestore** as its cloud database.

The database contains collections for the main system resources, including:

```text
schoolInformation
staffMembers
news
events
gallery
admissions
contactMessages
applications
```

Firestore provides a scalable cloud-based data storage solution and allows the API to retrieve and update information without relying on a local database.

---

# 🔐 Security

Security was considered throughout the implementation.

The website does not directly access the Firebase database.

Instead:

```text
Website
   ↓
API
   ↓
Firebase
```

Firebase Admin SDK credentials are not committed to the GitHub repository.

For local development, credentials are loaded through environment configuration.

For the hosted API, Firebase credentials are provided through secure Render environment variables.

Sensitive information such as:

* Firebase private keys
* Service account credentials
* Environment variables

is excluded from version control.

The `.gitignore` configuration includes Firebase credential files and `.env` files.

---

# ☁️ Deployment

The Node.js API is deployed using **Render**.

The API runs as a Node.js web service using:

```text
Build Command:
npm install
```

and:

```text
Start Command:
npm start
```

The hosted API allows the ASP.NET MVC website to communicate with the backend without relying on localhost.

The production architecture is therefore:

```text
User
 │
 ▼
Hosted ASP.NET MVC Website
 │
 │ HTTPS
 ▼
Hosted Node.js API
 │
 ▼
Firebase Firestore
```

This allows the system to operate as a client-ready hosted solution rather than only as a local development project.

---

# 🐙 Version Control

Git and GitHub are used for source-code management throughout the project.

The repository allows the development team to:

* Work collaboratively
* Create and manage branches
* Track changes
* Review development history
* Merge completed features
* Maintain previous versions of the system
* Coordinate frontend and backend development

Development is organised around individual features and responsibilities before being integrated into the shared system.

---

# 👥 Team Responsibilities

The project was developed collaboratively by:

### Moagi Theledi

**Project Manager / Developer**

Responsibilities include:

* Project coordination
* System architecture
* School Calendar
* School Gallery
* API integration
* Deployment coordination
* Technical documentation

### Tlhomamo

**UI/UX Designer / Developer**

Responsibilities include:

* Home page
* About Us page
* User interface design
* User experience considerations

### Nkumbu

**Backend Developer**

Responsibilities include:

* API development
* Admissions functionality
* Contact functionality
* Backend and database integration

### Oratile

**Frontend Developer**

Responsibilities include:

* News & Announcements
* Our Staff
* Frontend implementation
* API data integration

---

# 📱 Final Phase — Koos Matli Mobile Application

The current website represents the first major stage of the Koos Matli Primary School digital platform.

For the **final phase of the project**, the team will extend the system by developing a dedicated **Koos Matli Primary School mobile application using .NET MAUI**.

The mobile application will build on the existing backend infrastructure rather than creating a completely separate system.

The planned architecture is:

```text
                    USERS
                      │
              ┌───────┴────────┐
              │                │
              ▼                ▼
      ASP.NET MVC Website   .NET MAUI App
              │                │
              └───────┬────────┘
                      │
                      ▼
              Node.js REST API
                      │
                      ▼
              Firebase Firestore
```

This approach allows the website and mobile application to use the same backend and database.

---

# 📲 Planned .NET MAUI Application

The future Koos Matli App is intended to provide a more interactive and convenient experience for parents, learners, teachers, and school administrators.

Potential functionality includes:

* User authentication
* Parent accounts
* Learner information
* School announcements
* School calendar
* Push notifications
* Admission information
* Application tracking
* School contact functionality
* Staff information
* School gallery
* Important school resources
* Role-based functionality for different users

The exact functionality will be refined according to the requirements established for the final phase.

---

# 🔄 Reusing the Existing API

One of the main advantages of our current architecture is that the mobile application will be able to consume the existing REST API.

For example:

```text
                 /api/news
                    ▲
                    │
          ┌─────────┴─────────┐
          │                   │
     MVC Website         .NET MAUI App
```

Both clients can request the same information from the backend.

This prevents unnecessary duplication of business logic and database functionality.

For example, when a school administrator adds a new announcement, the same announcement can potentially become available to:

* The website
* The mobile application

without requiring the information to be entered twice.

---

# 🚀 Future Development

The final phase will focus on transforming the current website and API into a broader school digital ecosystem.

Future development will focus on:

1. Developing the **Koos Matli .NET MAUI mobile application**.
2. Extending the existing REST API where required.
3. Implementing authentication and authorisation.
4. Introducing role-based access for parents, teachers and administrators.
5. Adding mobile push notifications.
6. Improving application management.
7. Strengthening security and validation.
8. Expanding automated testing.
9. Improving monitoring and deployment processes.
10. Maintaining a shared backend between the website and mobile application.

---

# 🛠️ Technology Stack

## Current Website

| Technology       | Purpose                   |
| ---------------- | ------------------------- |
| ASP.NET Core MVC | Website framework         |
| C#               | Application development   |
| Razor            | Dynamic views             |
| HTML/CSS         | User interface            |
| JavaScript       | Client-side functionality |

## Current API

| Technology         | Purpose              |
| ------------------ | -------------------- |
| Node.js            | API runtime          |
| Express.js         | REST API framework   |
| Firebase Admin SDK | Firebase integration |
| REST               | API communication    |
| JSON               | Data exchange        |

## Database

| Technology         | Purpose        |
| ------------------ | -------------- |
| Firebase Firestore | Cloud database |

## Deployment & Development

| Technology              | Purpose                |
| ----------------------- | ---------------------- |
| Git                     | Version control        |
| GitHub                  | Source-code management |
| Render                  | API hosting            |
| Visual Studio / VS Code | Development            |

## Planned Final Application

| Technology         | Purpose                           |
| ------------------ | --------------------------------- |
| .NET MAUI          | Cross-platform mobile application |
| C#                 | Mobile application development    |
| Existing REST API  | Backend communication             |
| Firebase Firestore | Shared data source                |

---

# 📁 Project Structure

The repository contains the website and API as separate applications.

```text
KoosMatliWebsite/
│
├── KoosMatliWebsite/
│   ├── Controllers/
│   ├── DTOs/
│   ├── Services/
│   ├── Views/
│   ├── wwwroot/
│   ├── Program.cs
│   └── appsettings.json
│
└── KoosMatliWebsiteAPI/
    ├── config/
    │   └── firebase.js
    │
    ├── controllers/
    │   ├── aboutController.js
    │   ├── applicationsController.js
    │   ├── admissionsController.js
    │   ├── contactController.js
    │   ├── eventsController.js
    │   ├── galleryController.js
    │   ├── newsController.js
    │   └── staffController.js
    │
    ├── routes/
    │   ├── aboutRoutes.js
    │   ├── applicationsRoutes.js
    │   ├── admissionsRoutes.js
    │   ├── contactRoutes.js
    │   ├── eventsRoutes.js
    │   ├── galleryRoutes.js
    │   ├── newsRoutes.js
    │   └── staffRoutes.js
    │
    ├── server.js
    ├── package.json
    └── package-lock.json
```

---

# ▶️ Running the API Locally

Navigate to:

```bash
cd KoosMatliWebsite/KoosMatliWebsiteAPI
```

Install dependencies:

```bash
npm install
```

Create a `.env` file:

```env
PORT=5000
FIREBASE_CREDENTIAL_PATH=./your-firebase-service-account.json
```

Start the API:

```bash
npm start
```

The API will run locally on:

```text
http://localhost:5000
```

Health check:

```text
http://localhost:5000/health
```

---

# ▶️ Running the Website Locally

Navigate to the ASP.NET MVC project:

```bash
cd KoosMatliWebsite/KoosMatliWebsite
```

Restore dependencies:

```bash
dotnet restore
```

Run the application:

```bash
dotnet run
```

The website must be configured to communicate with the local or hosted API through:

```json
{
  "ApiSettings": {
    "BaseUrl": "http://localhost:5000/"
  }
}
```

For production, the BaseUrl will point to the deployed Render API.

---

# 📌 Current Project Status

### Website

**Completed**

* [x] Home Page
* [x] About Us
* [x] Our Staff
* [x] News & Announcements
* [x] School Calendar
* [x] School Gallery
* [x] Admissions
* [x] Contact
* [x] API integration
* [x] Firebase Firestore integration
* [x] Cloud API deployment
* [x] Production API configuration

### Backend

**Completed**

* [x] Node.js API
* [x] Express REST API
* [x] Firebase Admin SDK
* [x] Firestore integration
* [x] Resource-based endpoints
* [x] Form submission endpoints
* [x] Environment-based credentials
* [x] Render deployment

### Final Phase

**Planned**

* [ ] .NET MAUI mobile application
* [ ] Mobile authentication
* [ ] Role-based access
* [ ] Push notifications
* [ ] Parent functionality
* [ ] Teacher functionality
* [ ] Administrator functionality
* [ ] Extended API functionality
* [ ] Expanded automated testing
* [ ] Mobile deployment

---

# 🎓 Project Vision

The long-term goal of the **Koos Matli Primary School System** is to provide the school with a unified digital platform that connects its website, mobile application, communication channels, and school information.

The current website establishes the foundation for this ecosystem.

The final **.NET MAUI application** will build on that foundation by providing a dedicated mobile experience while continuing to use the existing backend and cloud infrastructure.

This approach allows the system to evolve from a public school website into a **scalable, maintainable and integrated school management and communication platform**.

# 🎓 YouTube link
This link takes you to a video that demonstrates the functionality of the website:
YouTube: https://youtu.be/H__yAlGE8Gg 

If the video is blurry, use this canva link to view the video:
Canva: https://canva.link/g1wh8bxtdopiysw 
