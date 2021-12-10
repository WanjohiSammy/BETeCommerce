# eCommerce Web App

This is an eCommerce Web app system.
The Project is build with the following technologies:
* React Typescript for front-end
* .NET Core 5.0 for back-end
* MSSQL Server

# Get Started
## Dependencies
In order to build and test this WebApp on local machine, install the following:
* node
* npm or yarn istalled golbally
* React 
* .NET Core 5.0
* git
* SQL Server Management Studio (SSMS) or any SQL Management Studio (MSSQL Server)

## Great Application to have
- Visual Studio IDE or use dotnet cli
- Visual Studio Code

## Installation
First clone the repo to have the code on your local machine
```sh
git clone https://github.com/WanjohiSammy/BETeCommerceApp.git
```

#### Back-End application
Restore .NET Core backages
```sh
You can use tool of your choice. I use Visual Studio IDE to restore and build
Install-Package
```
Create a Database.
```sh
Install Download SQL Server Management Studio (SSMS) or any SQL Management Studio
Run migrations to create tables in db. Migrations located in
cd ..\BETeCommerce\BETeCommerce.DataLayer\Migrations
If using Visual Studio IDE, make sure Default Project is "BETeCommerce.DataLayer"
```
```sh
using .bak file to restore DB. This contain some few data
located in: ..\BETeCommerce\BETeCommerceDB.bak
```
Modify the appsettings.json
```sh
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "Jwt": {
    "Key": "gzg5QFi0KHOhqrFVSHC1d6Ow6a4kqIlYKk8MhZ",
    "Issuer": "https://localhost:44352/",
    "Audience": "https://localhost:44352/"
  },
  "Email": {
    "FromAddress": "soft.happylife@gmail.com",
    "Password": "Password"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BETeCommerceDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```
Start back-end application. Swagger API page will open in the Browser
```
#### Front-End application
```sh
cd ...\BETeCommerce\ClientApp
```
To open with Visual Studio code if already installed
```sh
code .
```
Install react packages required for this app to start and run
```sh
yarn install
```
create .env in the root folder same as where the .gitgnore is.
Put this env variable in the .env file
https://localhost:44352/ is the back-end root url
```sh
REACT_APP_BASE_URL=https://localhost:44352/
```
Start App (Make sure the Back-End is running)
```sh
yarn start
```

## What it Can Do
> Register User
> User Login
> Show, Add, Update, Delete Product Category
> Show, Add, Update, Delete Products
> Add and remove from Cart
> Checkout
> Send Email

## Contact me
same.wanjohi94@gmail.com
