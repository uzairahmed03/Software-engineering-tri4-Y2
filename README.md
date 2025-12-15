# Software-engineering-tri4-Y2
**Overview**

Graphene Trace is a software application developed as part of a Software Engineering module. The system is designed to support clinical monitoring using pressure sensor data, enabling patients, clinicians, and administrators to interact with the system through role-specific dashboards.

The application allows:

Patients to view pressure heatmaps, trends, and leave feedback
Clinicians to monitor patient data, review alerts, and export metrics
Administrators to manage users and oversee system activity
The project is implemented using C# and ASP.NET Core for the backend, with HTML, CSS, and JavaScript used for the frontend dashboards.

**Project Structure**
GrapheneTrace/
│
├── GrapheneTrace/           # ASP.NET Core backend
│   ├── Controllers/         # API controllers
│   ├── Models/              # Database models
│   ├── Services/            # Business logic
│   ├── Data/                # DbContext and EF Core setup
│   └── Program.cs           # Application entry point
│
├── Frontend/                # HTML/CSS dashboards
│   ├── home.html
│   ├── admin-dashboard.html
│   ├── clinician-dashboard.html
│   └── patient-dashboard.html
│
├── README.md
└── .gitignore

**Technologies Used**
C# / ASP.NET Core
Entity Framework Core
SQLite
HTML, CSS, JavaScript
Git & GitHub

**How to Run the Project**
Prerequisites
.NET 7 SDK installed
Visual Studio Code or Visual Studio

**Steps to Run**
Open Visual Studio Code
Select the root project folder
Open a new terminal in VS Code
Change directory to the backend project:
cd GrapheneTrace
Run the application:
dotnet run

The application will start and the backend API will be available locally.
A SQLite database file will be created automatically if it does not already exist.

**Key Features**

Role-based dashboards (Admin, Clinician, Patient)
Pressure heatmap visualisation
Alert detection and monitoring
Patient messaging and comments
Metrics and trend analysis
Data export for clinical records

**Notes**
This project was developed as an academic submission and demonstrates core software engineering principles including MVC architecture, database normalisation, RESTful APIs, and modular design.
Further improvements could include authentication, automated testing, and cloud deployment.

GitHub Repository

https://github.com/uzairahmed03/Software-engineering-tri4-Y2
