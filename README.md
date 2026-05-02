# Client Scheduler

Client Scheduler is a Windows Forms desktop application for managing customers and appointments. It supports demo data for easy local review and MySQL mode for database-backed scheduling.

The app includes customer management, appointment scheduling, calendar views, reports, login tracking, business-hour validation, overlap prevention, time zone handling, and unit tests for core scheduling rules.

## Features

- Demo mode that runs without MySQL
- MySQL mode for database-backed scheduling
- Login screen with English and Spanish UI support
- Demo credentials for local review
- Customer management
- Appointment management
- Calendar view by selected day
- Reports for appointment and customer data
- Appointment alert on login for upcoming appointments
- Login history stored in the user's AppData folder
- Error logging stored in the user's AppData folder
- Validation for customer and appointment data
- Business-hour enforcement using Eastern Time
- Appointment overlap prevention
- Unit tests for validation and scheduling rules

## Tech Stack

- C#
- .NET Framework 4.8
- Windows Forms
- MySQL
- MySql.Data
- xUnit

## Screenshots

### Login

<img src="docs/assets/screenshots/login.png" alt="Login screen" width="500">

### Customers

<img src="docs/assets/screenshots/customers.png" alt="Customer management screen" width="500">

### Appointments

<img src="docs/assets/screenshots/appointments.png" alt="Appointment management screen" width="500">

### Reports

<img src="docs/assets/screenshots/reports.png" alt="Reports screen" width="500">

## How to Run

1. Clone the repository.

`git clone https://github.com/nencraft/scheduler-desktop-app.git`

2. Open the solution in Visual Studio.

`scheduler-desktop-app.sln`

3. Restore NuGet packages.

In Visual Studio:

`Right-click solution → Restore NuGet Packages`

4. Build the solution.

5. Run the `scheduler-desktop-app` project.

## Demo Login

Demo mode is enabled by default in `App.config`.

Use these credentials:

| Username | Password |
|---|---|
| `test` | `test` |

Demo mode loads sample customers and appointments from in-memory repositories, so MySQL is not required to review the app.

## Demo Mode and MySQL Mode

The app can run in two modes.

### Demo Mode

Demo mode is controlled by this setting in `App.config`:

`UseDemoData=true`

When demo mode is enabled, the app uses:

- `InMemoryUserRepository`
- `InMemoryCustomerRepository`
- `InMemoryAppointmentRepository`

This mode is intended for quick local review.

### MySQL Mode

To run against MySQL, set:

`UseDemoData=false`

Then provide a connection string using the `CLIENT_SCHEDULER_CONNECTION` environment variable.

Example:

`Server=localhost;Port=3306;Database=client_schedule;Uid=YOUR_DB_USER;Pwd=YOUR_DB_PASSWORD;`

If the environment variable is not set, the app falls back to the `localdb` connection string in `App.config`.

## Project Structure

```text
scheduler-desktop-app/
  Data/
    AppState
    Repository interfaces
    In-memory repositories
    MySQL repositories
    DemoDataSeeder

  Database/
    DBConnection

  Exceptions/
    AppointmentOperationException
    CustomerOperationException

  Localization/
    English and Spanish resource files

  Models/
    Appointment
    Customer
    Report rows

  Services/
    AppFileService
    AppointmentAlertService
    AppointmentValidationService
    ErrorLogService
    LoginHistoryService
    LocationService
    ReportService
    TimeService
    ValidationService

  Forms/
    LoginForm
    MainForm
    CustomerManagementForm
    CustomerEditForm
    AppointmentManagementForm
    AppointmentEditForm
    CalendarForm
    ReportsForm

scheduler-desktop-app.Tests/
  Services/
    AppointmentValidationServiceTests
    TimeServiceTests
    ValidationServiceTests
```

## Business Rules

### Customers

- Customer name is required.
- Address is required.
- Phone number is required.
- Phone numbers may contain digits, spaces, dashes, periods, parentheses, or a leading plus sign.
- Customers with appointments cannot be deleted until their appointments are deleted.

### Appointments

- Appointment type is required.
- End time must be after start time.
- Appointments must be within business hours.
- Business hours are Monday through Friday, 9:00 AM to 5:00 PM Eastern Time.
- Appointments cannot overlap for the same user.
- Appointment times are stored in UTC and displayed in the user's local time.

## Reports

The Reports screen includes:

- Appointment types by month
- Schedule by user
- Customer summary with appointment counts and next appointment dates

Reports are available from the main menu.

## App Data and Logging

The app stores generated files in the user's AppData folder under:

`ClientScheduler`

Stored files include:

- Login history
- Error logs

This keeps runtime files out of the project folder and avoids committing local app data.

## Testing

The solution includes an xUnit test project for non-UI business logic.

Run tests with:

`dotnet test scheduler-desktop-app.Tests/scheduler-desktop-app.Tests.csproj`

The tests cover:

- Business-hour validation
- Weekend appointment rejection
- Appointment end-time validation
- Appointment overlap validation
- Customer required-field validation
- Customer phone validation

## Recent Improvements

- Added demo mode with seeded in-memory data
- Added demo login using `test` / `test`
- Removed committed database credentials
- Added environment-variable support for MySQL connection strings
- Added AppData-based login history and error logging
- Added safer startup error handling
- Prevented deleting customers with existing appointments
- Added Reports to the main navigation
- Added appointment delete confirmation
- Improved customer phone validation
- Added unit tests for scheduling and validation logic
- Updated project documentation
