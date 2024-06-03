# Boilerplate-CRUD-dotnet-8
Boilerplate CRUD Web App created with .NET 8 by [StackPuz](https://stackpuz.com).

## Demo
Checkout the live demo at https://demo.stackpuz.com

## Features
- Fully Responsive Layout.
- Sorting, Filtering and Pagination on Data List.
- User Management, User Authentication and Authorization, User Profile, Reset Password.
- Input Mask and Date Picker for date and time input field with Form Validation and CSRF Protection.

![Responsive Layout](https://stackpuz.com/img/feature/responsive.gif)  
![Data List](https://stackpuz.com/img/feature/list.gif)  
![User Module](https://stackpuz.com/img/feature/user.png)  
![Input Mask and Date Picker](https://stackpuz.com/img/feature/date.gif)

## Minimum requirements
- .NET 8
- MySQL 5.7

## Installation
1. Clone this repository. `git clone https://github.com/stackpuz/Boilerplate-CRUD-dotnet-8.git .`
2. Create a new database and run [/database.sql](/database.sql) script to create tables and import data.
3. Edit the database configuration in [/appsettings.json](/appsettings.json) file.
    ```
    "ConnectionStrings": {
        "Database": "server=localhost;port=3306;database=testdb;user id=root;password=;charset=utf8;"
    }
    ```

## Run project

1. Run .NET project. `dotnet run --urls="http://localhost:5000"`
2. Navigate to `http://localhost:5000`
3. Login with user `admin` password `1234`

## Customization
To customize this project to use other Database Engines, CSS Frameworks, Icons, Input Mask, Date picker, Date format, Font and more. Please visit [stackpuz.com](https://stackpuz.com).

## License

[MIT](https://opensource.org/licenses/MIT)