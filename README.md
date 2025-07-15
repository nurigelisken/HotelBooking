# Hotel Room Booking API

This project is a hotel room booking system built with ASP.NET Core 8. 
It provides functionality to search for hotels, list available rooms, and make bookings with business rules such as avoiding double-booking or overcapacity.

## 🚀 Features

- Search hotels by name
- List available rooms between two dates and for a number of guests
- Make a booking with a unique reference
- Retrieve booking details by reference
- Seed and reset the database for testing purposes
- Swagger UI support
- Unit testing with xUnit, Moq, InMemory DB

## 🧱 Tech Stack

- ASP.NET Core 8 Web API
- Entity Framework Core
- SQL Server / InMemory
- xUnit + Moq
- Swagger (Swashbuckle)

### Clone the Repository
- Clone the repository to your local machine:
- Debug the project using Visual Studio or your preferred IDE.
- Swagger support (https://localhost:7138/swagger/index.html)


### Seed or Reset Database
- You can seed or reset the database by using
	- POST /api/admin/seed   → Seed with test data
    - POST /api/admin/reset  → Clear all data

