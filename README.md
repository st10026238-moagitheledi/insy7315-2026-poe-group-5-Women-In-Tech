# 🏫 Koos Matli Primary School Management System

### *Thuto ke Lesedi – Education is Light*

<p align="center">
  <strong>A centralised digital platform connecting Koos Matli Primary School, parents, teachers and administrators.</strong>
</p>

<p align="center">
  📱 .NET MAUI &nbsp; | &nbsp; ⚙️ Node.js &nbsp; | &nbsp; ☁️ Firebase
</p>

---

## 📑 Table of Contents

1. [📖 Project Overview](#-1-project-overview)
2. [🎯 Problem Statement](#-2-problem-statement)
3. [💡 Project Objectives](#-3-project-objectives)
4. [👥 Target Users and User Roles](#-4-target-users-and-user-roles)
5. [🌐 Website Overview](#-5-website-overview)
6. [📱 Mobile Application Overview](#-6-mobile-application-overview)
7. [🛠️ Technology Stack](#️-7-technology-stack)
8. [🏗️ System Architecture](#️-8-system-architecture)
9. [⚙️ System Requirements](#️-9-system-requirements)
10. [🔐 Security and Privacy](#-10-security-and-privacy)
11. [🚀 Installation and Setup](#-11-installation-and-setup)
12. [▶️ Running the Demonstration](#️-12-running-the-demonstration)
13. [🧪 Testing and Troubleshooting](#-13-testing-and-troubleshooting)
14. [🌍 Deployment Strategy](#-14-deployment-strategy)
15. [🔄 GitHub and Development Workflow](#-15-github-and-development-workflow)
16. [💰 Running Costs and Scalability](#-16-running-costs-and-scalability)
17. [🔧 Maintenance and Change Management](#-17-maintenance-and-change-management)
18. [🎥 Demonstration Video](#-18-demonstration-video)
19. [👨‍💻 Project Team](#-19-project-team)
20. [📌 Project Status](#-20-project-status)
21. [🙏 Acknowledgements](#-21-acknowledgements)

---

## 📖 1. Project Overview

The **Koos Matli Primary School Management System** is a digital solution developed to support communication, information sharing and administrative processes within Koos Matli Primary School, located in Mamelodi, South Africa.

The system consists of two complementary components:

- 🌐 **A public-facing school website:** Provides parents, prospective learners and members of the community with access to important school information.
- 📱 **A role-based mobile application:** Provides parents, teachers and administrators with access to relevant school information and management functionality.

The solution aims to create a more accessible and organised school environment by bringing important information and communication channels together in one digital platform.

The project is guided by the school's motto:

> **Thuto ke Lesedi – Education is Light.**

## 🎯 2. Problem Statement

Schools rely on effective communication and accurate information sharing between management, teachers, parents and learners.

When information is distributed across different communication channels or managed manually, it can become difficult for parents to keep track of announcements, school activities, learner information and important dates.

The Koos Matli Primary School Management System aims to address these challenges by providing a centralised digital platform that supports:

- Improved communication between the school and parents.
- Easier access to school announcements and important information.
- Better organisation of learner-related information.
- Convenient access to timetables, schedules and school events.
- More structured administrative processes.
- Improved accessibility through a multilingual application.

## 💡 3. Project Objectives

The main objectives of the system are to:

1. Develop a professional website that represents Koos Matli Primary School and provides essential public information.
2. Develop a mobile application that supports the daily information and communication needs of parents, teachers and administrators.
3. Provide role-based access so users only access functionality appropriate to their responsibilities.
4. Centralise school announcements, schedules, learner information and administrative resources.
5. Improve communication between parents and the school.
6. Support language accessibility through multiple language options.
7. Consider limited or expensive mobile data through Data Saver and offline-access considerations.
8. Apply appropriate security and privacy practices when handling school and learner information.
9. Provide a solution that can be maintained and expanded as the school's needs evolve.

## 👥 4. Target Users and User Roles

The mobile application supports three primary user roles.

### 👨‍👩‍👧 Parents

Parents use the application to stay informed about their children's education and school activities.

Key capabilities include:

- Registering a parent account.
- Logging into the application.
- Viewing the parent dashboard.
- Accessing linked learner profiles.
- Viewing school schedules and relevant timetables.
- Reading school announcements and updates.
- Accessing relevant academic and school information.
- Managing personal profile information and application settings.

### 👩‍🏫 Teachers

Teachers use the application to access relevant classroom information and support school communication.

Planned and intended capabilities include:

- Accessing assigned classes.
- Viewing relevant schedules and timetables.
- Accessing school announcements and updates.
- Sharing relevant learning-related information according to permissions.
- Managing relevant classroom information.
- Accessing personal account settings.

Teacher accounts are created and managed by authorised school personnel rather than through unrestricted teacher registration.

### 🛡️ Administrators

Administrators are responsible for managing relevant school information and application users.

Key capabilities include:

- Managing parent, teacher and administrator accounts.
- Reviewing and managing learner admission applications.
- Managing relevant school information.
- Maintaining school announcements and content.
- Accessing administrative settings.
- Managing information according to assigned permissions.

Sensitive administrative actions, such as changing school fee information, must be restricted to the appropriate authorised roles. Teachers should not have permission to modify school fees.

---

## 🌐 5. Website Overview

The public-facing website serves as the school's online information and communication platform.

It is designed to provide a professional digital presence for Koos Matli Primary School and make essential information accessible to parents, prospective learners and the wider community.

### Website Sections

| Page | Description |
|---|---|
| 🏠 Home | Introduces the school, its identity, motto and important information. |
| 🏫 About Us | Presents the school's background, mission, vision and values. |
| 👩‍🏫 Our Staff | Introduces staff members, including their names, roles and relevant information. |
| 📝 Admissions | Provides information about the school's admission process and requirements. |
| 📢 News and Announcements | Shares school notices, updates and important communications. |
| 🖼️ School Gallery | Displays photographs and highlights school activities and events. |
| 📍 Find Us / Contact | Provides the school's location, contact information and directions. |
| 📅 School Calendar | Displays important school dates, activities and events. |

### Website Design Considerations

The website is intended to:

- Maintain a consistent school identity and professional appearance.
- Use the school's navy and gold colour palette.
- Provide clear navigation and readable content.
- Adapt to desktop, tablet and mobile screen sizes.
- Make important school information easy to locate.
- Provide accessible and user-friendly layouts.

---

## 📱 6. Mobile Application Overview

The mobile application provides a role-based experience for parents, teachers and administrators. (found in dev_app branch)

### 🌍 Language Selection

The application supports three language options:

- English
- Sepedi
- Setswana

Users can select their preferred language and access language preferences through the application's settings.

### 🔑 Login and Registration

The login section provides secure access to the application according to the user's assigned role.

- Parents can register through the application.
- Teacher and administrator accounts are managed by the school.
- Users are directed to the relevant interface according to their assigned role.
- Access to sensitive functionality is restricted according to permissions.

### 👨‍👩‍👧 Parent Navigation

The Parent interface uses a bottom navigation bar.

| Section | Purpose |
|---|---|
| 🏠 Dashboard | Provides an overview of relevant school information, updates and upcoming activities. |
| 📅 Schedule | Allows parents to access relevant timetables, school schedules and events. |
| 📢 Updates | Provides access to school announcements, notices and newsletters. |
| 👨‍👩‍👧 Children | Displays learner profiles linked to the parent's account and relevant learner information. |
| ⚙️ Profile and Settings | Allows parents to manage their profile and application preferences. |

### 👩‍🏫 Teacher Navigation

The Teacher interface provides access to relevant classroom and communication tools.

| Section | Purpose |
|---|---|
| 🏠 Dashboard | Provides an overview and starting point for teacher functionality. |
| 📚 Classes | Supports access to assigned classes and class-related information. |
| 📢 Updates | Provides access to school communications and relevant learning-related updates. |
| 📅 Schedule | Provides access to teaching schedules, timetables and school events. |
| 👤 Profile | Allows teachers to access their account information and preferences. |

### 🛡️ Administrator Navigation

The Administrator interface provides tools for managing the school's digital information.

| Section | Purpose |
|---|---|
| 🏠 Dashboard | Provides an overview of administrative functionality. |
| 👥 Users | Supports the management of application accounts and user access. |
| 📝 Applications | Provides access to learner admission and application information. |
| 🏫 School | Supports the management of relevant school information and content. |
| ⚙️ Settings | Provides access to administrative and application preferences. |

### 📶 Accessibility and Data Usage

The system considers the connectivity and data challenges experienced by some members of the school community.

The project therefore considers:

- Data Saver Mode to help reduce unnecessary data usage.
- Offline-access considerations for relevant information.
- Clear navigation and straightforward user interactions.
- Multilingual support to improve accessibility.

The availability of individual offline and data-saving features depends on their implementation in the current application build.

---

## 🛠️ 7. Technology Stack

The system uses the following technologies:

| Technology | Purpose |
|---|---|
| 📱 .NET MAUI | Development of the cross-platform mobile application. |
| ⚙️ C# | Application logic and functionality within the .NET MAUI project. |
| 🎨 XAML | Mobile application user interface design. |
| 🟢 Node.js | Backend runtime for the application API and server-side logic. |
| 🔥 Firebase | Cloud services and data storage used by the system. |
| 🔌 REST API | Communication between the mobile application and backend services. |
| 🧑‍💻 Visual Studio | Development and execution of the .NET MAUI application. |
| 💻 Visual Studio Code | Backend API development and execution. |
| 🐙 GitHub | Source-code management, collaboration and project version control. |

### Technology Responsibilities

**.NET MAUI**

Provides the mobile application's user interface, navigation and client-side functionality.

**Node.js**

Runs the backend API, processes application requests and handles relevant business logic.

**Firebase**

Provides cloud-based services and data storage for the system.

**REST API**

Enables communication between the mobile application and backend, allowing the application to request and process relevant information.

---

## 🏗️ 8. System Architecture

The mobile application follows a client-server architecture.

The general communication flow is:

```text
             👤 USER
                |
                v
       📱 .NET MAUI APPLICATION
          (C# and XAML)
                |
                | HTTP API Requests
                v
          ⚙️ NODE.JS API
       (Backend and Business Logic)
                |
                | Firebase Integration
                v
           🔥 FIREBASE
       (Cloud Data and Services)
```

### Architecture Explanation

1. The user interacts with the .NET MAUI mobile application.
2. The application sends relevant requests to the Node.js API.
3. The Node.js backend processes requests and applies the relevant business logic.
4. The backend communicates with Firebase to retrieve or manage relevant information.
5. The API returns the appropriate response to the mobile application.
6. The application displays the information to the user.

The website and mobile application form part of the wider school digital solution. Their respective hosting and integration arrangements should be configured according to the final deployment architecture.

---

## ⚙️ 9. System Requirements

The system is designed around functional and non-functional requirements.

### Functional Requirements

The solution aims to provide:

- Public access to essential school information through the website.
- Parent registration and login.
- Role-based access for parents, teachers and administrators.
- Multilingual application support.
- Parent access to linked learner information.
- Access to school schedules and timetables.
- School announcements and updates.
- Teacher access to relevant classroom information.
- Administrative management of users and applications.
- Management of relevant school information.
- Controlled access to sensitive administrative functionality.
- Communication between the mobile application, API and Firebase.

### Non-Functional Requirements

| Requirement | Description |
|---|---|
| 🔐 Security | Protect user accounts and school information through appropriate authentication, authorisation and secure communication. |
| ⚡ Performance | Ensure that pages and application features respond efficiently. |
| 📱 Usability | Provide intuitive navigation and a user-friendly experience. |
| ♿ Accessibility | Consider readability, language accessibility and inclusive interface design. |
| 📶 Availability | Ensure that the system and its services are accessible when required. |
| 📈 Scalability | Allow the system to support growth in users and school information. |
| 🔧 Maintainability | Structure the application so that future changes and improvements can be managed. |
| 🔄 Reliability | Handle errors and unexpected situations without unnecessarily disrupting the user experience. |
| 🌍 Compatibility | Support the intended devices and screen sizes. |
| 📉 Data Efficiency | Consider limited connectivity and reduce unnecessary data usage where possible. |

---

## 🔐 10. Security and Privacy

The system handles information relating to parents, learners, teachers and school administration. Security and privacy are therefore important considerations.

The project considers the following practices:

- **Authentication:** Users must sign in before accessing protected areas.
- **Authorisation:** Users should only access functionality permitted by their assigned roles.
- **Role-based access control:** Parent, teacher and administrator permissions must be appropriately separated.
- **Data protection:** Sensitive school and learner information must be protected from unauthorised access.
- **Secure communication:** Production communication should use HTTPS/TLS.
- **Input validation:** User input must be validated to reduce invalid or malicious requests.
- **Error handling:** Errors should be handled without exposing sensitive technical information.
- **Credential protection:** API keys, service credentials and secrets must not be committed to GitHub.
- **POPIA considerations:** Personal information must be collected, processed, stored and accessed responsibly in accordance with the Protection of Personal Information Act.

Access to learner information must be limited to authorised users with an appropriate relationship or responsibility.

Security controls should be verified against the actual implementation before production deployment.

---

## 🚀 11. Installation and Setup

<details>
<summary><strong>📌 Important: Read before running the application</strong></summary>

This repository contains a demonstration/testing version of the system.

The mobile application depends on the Node.js API to retrieve and process backend information.

**The API must be running before launching the .NET MAUI application.**

For the current demonstration:

1. Run the Node.js API in Visual Studio Code.
2. Confirm that the API starts successfully.
3. Open the .NET MAUI project in Visual Studio.
4. Build and run the mobile application.

The mobile application is not currently distributed as a final downloadable production application. The final deployment will include a prepared application package for users to install.

</details>

### 11.1 Prerequisites

Install the following development tools before starting:

| Tool | Purpose |
|---|---|
| 🟢 Node.js and npm | Required to install and run the backend API. |
| 💻 Visual Studio Code | Used to open and run the Node.js API. |
| 🟣 Visual Studio | Used to open, build and run the .NET MAUI project. |
| 📱 .NET MAUI workload | Required for mobile application development. |
| 🐙 Git | Required to clone and manage the repository. |
| 🔥 Firebase project configuration | Required for the backend's Firebase connection. |

A compatible .NET SDK and the required Visual Studio MAUI workloads must also be installed.

### 11.2 Clone the Repository

Open a terminal and run:

```bash
git clone <YOUR_GITHUB_REPOSITORY_URL>
```

Navigate into the project folder:

```bash
cd <YOUR_PROJECT_FOLDER>
```

### 11.3 Step One: Run the Node.js API in Visual Studio Code

**The backend API must be started first.**

1. Open Visual Studio Code.
2. Select **File → Open Folder**.
3. Open the backend/API project folder.
4. Open the integrated terminal using **Terminal → New Terminal**.
5. Navigate to the API directory if you are not already in it.

Install the backend dependencies:

```bash
npm install
```

Before starting the API, ensure that the required environment configuration is available.

The Firebase configuration and other required environment variables must be set up using the project's configuration instructions. Do not commit private credentials or `.env` files containing secrets.

Start the API using the script defined in the backend's `package.json`.

For example, if the project defines a development script:

```bash
npm run dev
```

Alternatively, if the project uses a start script:

```bash
npm start
```

**Important:** Use the command that is actually defined in the API project's `package.json`. These commands are alternatives, not commands that must both be executed.

Wait until the terminal confirms that the server has started successfully.

Keep the API terminal open while testing the mobile application.

### 11.4 Step Two: Run the .NET MAUI Application in Visual Studio

Once the API is running:

1. Open **Visual Studio**.
2. Select **Open a project or solution**.
3. Navigate to the mobile application folder.
4. Open the `.sln` solution file, or the `.csproj` project file if no solution file is provided.
5. Allow Visual Studio to restore the required NuGet packages.
6. Confirm that the API base URL configured in the application points to the running backend.
7. Select the appropriate target device or emulator.
8. Build the solution.
9. Run the application using the Start button or `F5`.

The mobile application should now be able to communicate with the running API.

### 11.5 API Connection Configuration

The mobile application must use the correct API base address.

The API address depends on the environment in which the application is running.

For example:

- Android emulator: commonly uses `10.0.2.2` to access the host machine's localhost.
- Windows application: may use the local machine's configured localhost address.
- Physical mobile device: may require the computer's local network IP address, with both devices connected to the same network.

Use the actual API port and protocol configured in the project.

If the application cannot retrieve information, first confirm that the API is running and that the configured base URL is reachable from the selected target device.

### 11.6 Website Setup

The website is included as part of the wider system.

To run it locally:

1. Open the website project folder.
2. Review the website's project files and setup instructions.
3. Install any dependencies required by its framework, if applicable.
4. Configure any required environment settings.
5. Run the website using the development command appropriate to its actual technology stack.

The website's exact start command depends on the framework and scripts included in its project folder.

---

## ▶️ 12. Running the Demonstration

For the current testing-phase demonstration, follow this sequence:

| Order | Application | Action |
|---|---|---|
| 1️⃣ | Node.js API | Open the backend in Visual Studio Code and start the API. |
| 2️⃣ | API | Confirm that the backend is running successfully. |
| 3️⃣ | .NET MAUI | Open the mobile application in Visual Studio. |
| 4️⃣ | .NET MAUI | Build and launch the application on the selected emulator or device. |
| 5️⃣ | Mobile App | Select a language and log in using a demonstration account. |
| 6️⃣ | Mobile App | Explore the Parent, Teacher and Admin interfaces. |

### Demonstration Accounts

Use the demonstration credentials supplied by the project team.

Do not publish passwords, private account details or real learner information in this repository.

---

## 🧪 13. Testing and Troubleshooting

The system should be tested to ensure that its main functionality, API communication and user access controls operate as expected.

### Testing Areas

- Login and registration.
- Role-based navigation and access.
- Parent dashboard information.
- Linked learner information.
- Announcements and updates.
- Schedule and timetable information.
- Teacher classroom functionality.
- Administrator user and application management.
- API connectivity.
- Firebase data retrieval and updates.
- Error handling and validation.
- Website navigation and responsive layouts.

### Common Problems

| Problem | Possible Solution |
|---|---|
| Mobile app cannot connect to the API | Confirm that the API is running and the base URL is correct. |
| API fails to start | Check Node.js installation, dependencies and the start script in `package.json`. |
| Firebase connection fails | Verify Firebase configuration and service credentials. |
| Login does not work | Check the account credentials, authentication configuration and API response. |
| Data does not load | Check API connectivity, endpoint configuration and Firebase data. |
| Android emulator cannot reach localhost | Check whether the emulator requires the host address `10.0.2.2`. |
| Build errors in Visual Studio | Restore NuGet packages and verify the installed .NET MAUI workloads. |
| Website does not load | Check the website's development server, dependencies and configuration. |

---

## 🌍 14. Deployment Strategy

### Current Stage: Testing and Demonstration

The current version is intended for development, testing and demonstration purposes.

The system is run locally using the development environments:

- **Backend:** Node.js API running through Visual Studio Code.
- **Mobile application:** .NET MAUI application running through Visual Studio.
- **Cloud services:** Firebase configuration connected to the backend.

This setup allows the project team to demonstrate the system's functionality without presenting the current build as a fully deployed production solution.

### Planned Final Deployment

For final deployment, the objective is to make the mobile application available as a downloadable application package.

The intended deployment process includes:

1. Complete and verify the mobile application's functionality.
2. Complete backend testing and configuration.
3. Prepare production Firebase configuration and security rules.
4. Deploy the Node.js API to a suitable hosting environment.
5. Configure secure production API endpoints.
6. Host the public-facing school website.
7. Build and package the .NET MAUI application for the intended mobile platform.
8. Make the application available to authorised users for download and installation.
9. Perform final security, usability and compatibility testing.
10. Provide the school with the necessary documentation and user guidance.

The final production environment will not require users to open Visual Studio Code or Visual Studio to use the application.

---

## 🔄 15. GitHub and Development Workflow

GitHub is used to manage the project's source code and support collaboration between team members.

### Recommended Branch Structure

```text
main
 │
 ├── development
 │     │
 │     ├── feature/website
 │     ├── feature/mobile-app
 │     ├── feature/backend-api
 │     └── feature/admin
 │
 └── release
```

### Development Practices

- Use feature branches for individual functionality.
- Commit changes regularly with meaningful commit messages.
- Merge completed and reviewed changes into the development branch.
- Test changes before merging into the main or release branch.
- Keep sensitive credentials and configuration secrets out of the repository.
- Maintain clear documentation for setup and deployment.

### Continuous Integration and Deployment

GitHub Actions can be used to automate development workflows, including:

- Building the backend.
- Building the mobile application.
- Running automated tests.
- Validating code changes.
- Preparing release builds.
- Supporting deployment after approved changes are merged.

Any automated workflows should be documented and kept aligned with the actual GitHub Actions configuration in the repository.

---

## 💰 16. Running Costs and Scalability

The system uses Firebase and requires a hosting environment for the backend API and public-facing website.

Potential operational costs include:

| Component | Cost Consideration |
|---|---|
| Firebase | Database usage, authentication and other enabled cloud services. |
| Node.js API Hosting | Server or platform hosting costs. |
| Website Hosting | Website hosting and domain registration. |
| Domain | Annual domain registration and renewal. |
| Storage | Storage requirements for school documents and images. |
| Network Usage | Data transfer and bandwidth requirements. |
| Maintenance | Ongoing updates, monitoring and technical support. |

Actual costs will depend on the selected hosting providers, Firebase services, usage levels and the number of users.

The solution should be monitored as usage grows to ensure that infrastructure costs remain sustainable for the school.

---

## 🔧 17. Maintenance and Change Management

The system is intended to support the school's evolving requirements.

Future maintenance and change management activities may include:

- Updating school information and contact details.
- Adding new application features.
- Updating language translations.
- Improving performance and data efficiency.
- Resolving reported bugs.
- Updating security configurations.
- Maintaining Firebase data structures.
- Updating application packages.
- Providing user training and support.
- Reviewing feedback from parents, teachers and administrators.

A formal handover should include the source code, deployment instructions, configuration guidance, administrative access procedures and user documentation.

---

## 🎥 18. Demonstration Video

Watch the demonstration of the Koos Matli Primary School Management System:

▶️ **[Watch the System Demonstration Video](<INSERT_DEMONSTRATION_VIDEO_LINK_HERE>)**

The demonstration video showcases the system's main interfaces and explains the functionality available to parents, teachers and administrators.

> Replace the placeholder above with the final video URL before publishing the README.

---

## 👨‍💻 19. Project Team

This project was developed collaboratively as part of the Work-Integrated Learning (WIL) programme.

| Team Member | Role | Main Responsibility |
|---|---|---|
| **Moagi Theledi** | Project Manager | Project coordination, planning, team communication and project oversight. |
| **Nkumbu** | Backend Developer | Node.js API, backend functionality and Firebase integration. |
| **Oratile** | Frontend Developer | Application frontend implementation and interface functionality. |
| **Tlhomamo** | UI/UX Designer | User experience, interface design and visual consistency. |

---

## 📌 20. Project Status

<details>
<summary><strong>Current development status</strong></summary>

The project is currently maintained in a testing and demonstration phase.

The website, backend API and .NET MAUI application form part of the overall solution. The current demonstration setup requires the backend API to be started locally before launching the mobile application.

The final deployment phase will focus on production hosting, final testing, application packaging and making the mobile application available for download.

</details>

| Component | Current Purpose |
|---|---|
| 🌐 Website | Public-facing school information platform. |
| 📱 .NET MAUI App | Mobile application for parents, teachers and administrators. |
| ⚙️ Node.js API | Backend services and application communication. |
| 🔥 Firebase | Cloud services and data storage. |
| 🧪 Current Environment | Local testing and demonstration. |
| 🚀 Future Environment | Hosted production services and downloadable mobile application. |

---

## 🙏 21. Acknowledgements

We would like to acknowledge **Koos Matli Primary School** for providing the project context and the opportunity to develop a digital solution aimed at supporting the school's community.

We also acknowledge the guidance and support received throughout the Work-Integrated Learning process.

This project represents our collective effort to apply software development principles, teamwork, problem-solving and technical knowledge to a real-world organisational need.

---

<p align="center">
  🏫 <strong>Koos Matli Primary School Management System</strong>
  <br>
  <em>Thuto ke Lesedi – Education is Light.</em>
  <br><br>
  Built with ❤️ by the project team.
</p>
