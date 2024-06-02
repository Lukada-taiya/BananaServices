# BananaServices

Banana Services is a robust and scalable microservices-based application using .NET Core 8. The application implements seven distinct microservices, each designed to perform specific functions while communicating asynchronously and synchronously to ensure seamless operation.

Features
- Utilizes MVC architecture to ensure a clean separation of concerns within each microservice
- Implements secure .NET APIs for each microservice, incorporating authentication and authorization mechanisms.
- Implements role-based authorization to control access to various parts of the application based on user roles
- Implements both asynchronous and synchronous communication patterns between microservices.
- Integrates Azure Service Bus for reliable message queuing and topic-based publish/subscribe messaging.
- Uses Entity Framework Core for object-relational mapping (ORM) to interact with a SQL Server database.
- Implements API gateways to manage and route traffic to the appropriate microservice.
- Use Ocelot as the API gateway to manage requests from clients and route them to the appropriate microservice.
- Uses N-layer architecture to organize the application into logical layers, enhancing separation of concerns.
- Develops a user-friendly ASP.NET Core web application using Bootstrap 5 for responsive design.

## Installation

After cloning the project, use visual studio nuget package manager [Nuget Package Manager](https://learn.microsoft.com/en-us/nuget/consume-packages/install-use-packages-visual-studio) to install all dependencies of this microservice.
This project uses Sql Server

#### Requirements
The following dependencies is required to run BananaServices
- Sql Server
- Visual Studio (optional)
- Git 

#### Setup
To setup run these commands:
> git clone https://github.com/Lukada-taiya/BananaServices.git

After cloning the project, you will need to configure each Api Service's appsettings.json
- In the Services folder, navigate to each Api class library
  
  ![image](https://github.com/Lukada-taiya/BananaServices/assets/60792883/0e0076ac-e4df-4c88-8165-ab4fcbd2b105)

- Open each appsettings.json file to configure the default connection to match your machine.

  ![image](https://github.com/Lukada-taiya/BananaServices/assets/60792883/750aa11d-16d2-44c1-89e2-2e5119ef17a5)

- In the package manager console, run this command to setup the database tables
  > update-database


## Usage
Click on F5 on your keyboard to run the Service. All Available APIs will displayed.

## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

Please make sure to update tests as appropriate.
 
