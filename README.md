# AutoParts Management System

A web app for managing an auto parts store: products, customers, sales and staff.
I built it to learn ASP.NET Core and to practice working with real business logic.

![Dashboard](docs/screenshots/dashboard.png)

## What it does

- **Products and categories** - add, edit and delete parts. Each part has a unique part number, and a product that was already sold can't be deleted.
- **Customers** - keep a list of customers with their number of orders and total spent.
- **Sales (POS)** - the cashier adds products to a cart, picks a customer (or a walk-in sale), a discount and a payment method, and completes the sale.
- **Orders** - see all sales and open the details of each receipt.
- **Dashboard** - today's sales, orders for the last 7 days, low stock products and recent sales.
- **Users and roles** - the admin creates staff accounts, changes roles, blocks users and resets passwords.

## Screenshots

| Sales (POS) | Order details |
|---|---|
| ![Sales](docs/screenshots/sales.png) | ![Order details](docs/screenshots/order-details.png) |

| Products | Users |
|---|---|
| ![Products](docs/screenshots/products.png) | ![Users](docs/screenshots/users.png) |

## Tech stack

- C# / ASP.NET Core MVC (.NET 10)
- Entity Framework Core (Code First, migrations)
- SQL Server (LocalDB)
- ASP.NET Core Identity
- Bootstrap 5

The UI was designed in Figma and turned into HTML/CSS. My main focus in this project was the backend.

## Roles

| Page | Admin | Manager | Cashier |
|---|---|---|---|
| Dashboard, Sales, Orders | yes | yes | yes |
| View products | yes | yes | yes |
| Add / edit / delete products | yes | yes | no |
| Categories | yes | yes | no |
| Add / edit customers | yes | yes | yes |
| Delete customers | yes | yes | no |
| Users | yes | no | no |

There is no public registration. Only the admin can create new users.

## A few things I'm proud of

**Selling the last item.** When two cashiers try to sell the last item at the same time, only one sale goes through. I used a `RowVersion` column (optimistic concurrency) for this.

**One transaction per sale.** The order, its items and the stock update are saved together. If something fails, nothing is saved.

**Prices are saved in the receipt.** Each order item keeps the price from the moment of the sale, so old receipts don't change when a product's price changes.

**Not trusting the browser.** Prices and totals are calculated on the server, and stock is checked on the server, so changing values in the browser (F12) doesn't work.

**Basic security.** Passwords are stored as hashes by Identity, forms are protected against CSRF, accounts get locked after several wrong passwords, and login redirects only go to pages inside the app.

## How to run it

1. Clone the repository:
   ```
   git clone https://github.com/hotbyte225/AutoPartsManagementSystem.git
   ```
2. Check the connection string in `appsettings.json`. By default it uses SQL Server LocalDB.
3. Set the first admin account with user secrets (from the project folder):
   ```
   dotnet user-secrets set "Seed:AdminEmail" "admin@example.com"
   dotnet user-secrets set "Seed:AdminPassword" "YourStrong!Password1"
   ```
4. Create the database:
   ```
   dotnet ef database update
   ```
5. Run the app:
   ```
   dotnet run
   ```
6. Log in with the admin email and password from step 3.

When the app starts, it creates the Admin, Manager and Cashier roles and the admin account if they don't exist yet.

## What's next

- Move all database calls to `async`
- Move the sales logic from the controller to a service
- Add unit tests for the checkout
- Add a REST API
- Deploy a live demo
- Make a desktop version with WinForms

## About me

I'm Zufar Salayev, a .NET student at PDP University in Tashkent.
[LinkedIn](https://www.linkedin.com/in/zufar-salaev-1a9b1a238)
