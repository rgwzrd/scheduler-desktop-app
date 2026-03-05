# Client Scheduler

A Windows Forms scheduling app backed by MySQL. It supports managing customers and appointments, enforces scheduling rules (business hours + no overlaps), handles time zones/DST correctly, and includes calendar + reporting views.

## Features
- **Login**
  - English/Spanish UI
  - Detects user region/time zone
  - Authenticates against the MySQL `user` table (`test` / `test`)
  - Alerts on login if an appointment starts within 15 minutes
  - Appends successful logins to `Login_History.txt`

- **Customers**
  - Add / Update / Delete
  - Validation: required fields, trimmed input, phone format (digits + dashes)
  - Exception handling around database operations

- **Appointments**
  - Add / Update / Delete
  - Linked to customers, includes appointment type
  - Validation:
    - Mon–Fri, 9:00–17:00 Eastern Time
    - Prevents overlapping appointments (per user)
  - Stores times in UTC and displays them in the user’s local time (DST-safe)

- **Calendar**
  - Month view + day selection
  - Filters appointments by selected day

- **Reports (LINQ / lambdas)**
  - Appointment types by month
  - Schedule per user
  - Customer summary report (counts + next appointment)

## Tech
- C# / .NET Framework (WinForms)
- MySQL (`client_schedule` schema)
- `MySql.Data` connector (parameterized SQL; no ORM)

## Notes
- Appointment times are stored as UTC in the database and converted for display/validation using `TimeZoneInfo`.
- The app expects the `client_schedule` schema and a DB user `sqlUser` / `Passw0rd!` as commonly used in the WGU environment.