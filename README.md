# UdcSoft

This is an inventory and sales control system developed in C# using Windows Forms and SQL Server. The application allows you to manage product stock, clients, categories, users, register sales, and generate reports.

## Features

- **Inventory Management**: Add, edit, and delete products from the inventory.
- **Sales Registration**: Register sales and automatically update the stock.
- **Reports**: Generate sales and inventory reports.
- **Search and Filtering**: Search for products by name, category, or code.
- **Authentication**: Add and remove users for restricted access via login.

## Technologies Used

- **C#**: Programming language for the back-end development.
- **Windows Forms**: Framework for the user interface (UI).
- **SQL Server**: Database for storing product, sales, and user information.

## Prerequisites

- **.NET Framework** 4.7.2 or higher
- **SQL Server** (compatible version)
- **Visual Studio** (to compile and run the project)

## Installation

1. **Clone the repository**:
   
2. **Database Setup**:
   - Import or create the `database.sql` file to create the tables in SQL Server.
   - Update the connection string in the `SqlConnection` in the Windows Forms files to match your database.

3. **Open and Compile the Project**:
   - Open the project in Visual Studio.
   - Compile and run the project.

## Usage

1. When the application opens, you will be presented with a login screen (the login credentials must already exist in your database).
2. After logging in, you can access the main menu, where you'll find options to:
   - Manage Product Inventory.
   - Manage Clients.
   - Manage Categories.
   - Manage Users.
   - Manage Orders.
   
## Contributing

If you wish to contribute improvements or fixes, send me an email!
