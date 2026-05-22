# JobPortal

JobPortal is a web-based recruitment platform built using ASP.NET MVC and Oracle Database. The application enables employers to post job opportunities and applicants to search, apply, and track applications through a secure authentication system.

## Features
- User registration and login authentication
- Secure role-based access for employers and applicants
- Job posting and management
- Job search and application tracking
- Employer and applicant dashboards
- Database integration for storing user, job, and application details
- Organized development using MVC architecture

## Technologies Used
- **Frontend:** HTML, CSS, Razor Views (.cshtml), Bootstrap
- **Backend:** C#, ASP.NET MVC
- **Database:** Oracle Database (managed using Oracle SQL Developer)
- **IDE/Tools:** Visual Studio
- **Architecture:** MVC (Model-View-Controller)

## Installation

1. Clone the repository:

```bash
git clone <repository-link>
```

2. Navigate to the project directory:

```bash
cd JobPortal
```

3. Open the project in Visual Studio.

4. Configure the Oracle Database connection string.

5. Build and run the application.

## Usage

- Register/Login as an employer or applicant.
- Employers can create, update, and manage job postings.
- Applicants can browse available jobs and apply for positions.
- Users can track application status through the portal.

## Project Structure

```plaintext
JobPortal/
│── Controllers/      # Handles application logic
│── Models/           # Database models and entities
│── Views/            # Razor (.cshtml) UI pages
│── wwwroot/          # Static files (CSS, JS, images)
│── App_Data/         # Application data/configuration
│── Program.cs / Startup.cs
```

## Future Improvements
- Resume upload functionality
- Email notifications for applications
- Interview scheduling system
- AI-based job recommendations
- Enhanced dashboard analytics

## Author

Developed by Vedant Chaudhari
