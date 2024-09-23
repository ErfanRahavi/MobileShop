MobileShop
MobileShop is a web-based API built using ASP.NET Core 8.0 and Entity Framework Core, designed to manage mobile phone inventories, user authentication, and order processing. The API supports secure authentication using JWT tokens, role-based authorization, and various operations for admins and users.

Features:

User Management
Register and Login functionality for users and admins.
Role-based access control with Admin and User roles.

Mobile Phone Management (Admin Only)
Create, Update, Delete, and View mobile phone details.
Manage mobile phone stock levels (Increase/Decrease stock).

Order Management
Users can place orders for available mobile phones.
Admins can view all user orders, while users can view only their own order history.

Mobile Search
Users can search for mobile phones with filters like price, year, and brand.

API Endpoints
Authentication Endpoints
POST /api/authenticate/register: Register a new user.
POST /api/authenticate/login: Login to get a JWT token.

Mobile Management (Admin Only)
GET /api/mobile: List all mobile phones.
POST /api/mobile: Add a new mobile phone.
PUT /api/mobile/{id}: Update mobile phone details.
DELETE /api/mobile/{id}: Remove a mobile phone.
POST /api/stock/increase/{id}: Increase stock for a mobile.
POST /api/stock/decrease/{id}: Decrease stock for a mobile.

Orders
POST /api/orders: Create a new order.
GET /api/orders/myorders: Get order history for the logged-in user.
GET /api/orders/allorders: Admin access to view all orders.

Mobile Search
GET /api/mobilesearch: Search mobiles with filters.

Technologies Used 
ASP.NET Core 8.0
Entity Framework Core (with SQL Server)
JWT Authentication & Authorization
Serilog for logging
Swagger for API documentation
Visual Studio 2022

Setup and Installation
Clone the repository:
git clone https://github.com/ErfanRahavi/MobileShop.git
cd MobileShop

Configure the database connection in appsettings.json:
"ConnectionStrings": {
  "ConnStr": "Your-Database-Connection-String"
}

Apply database migrations:
dotnet ef database update

Run the project:
dotnet run

Access the API documentation via Swagger:
https://localhost:7128/swagger/index.html
