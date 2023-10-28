# Microservices App

This repository contains a microservices application built using .NET Core 7. The application consists of various services that work together to provide different functionalities. The services included in this application are:

1. API Gateway (Ocelot): A reverse proxy and API gateway that handles all the external client requests and routes them to the appropriate microservices.
2. Azure Service Bus (Queue, Topic, and Subscription): A message broker service provided by Azure that enables asynchronous communication between different microservices.
3. Auth API Service: A service responsible for handling user authentication and authorization.
4. Coupon API Service: A service that manages coupon functionality for the application.
5. Product API Service: A service that manages the products in the application.
6. Shopping Cart API Service: A service that handles all the operations related to a user's shopping cart.
7. Order API Service: A service responsible for managing the order processing and fulfillment.
8. Email API Service: A service that handles sending email notifications to users.
9. Payment Gateway: A payment processing service integrated with the application to handle payment transactions.
10. Reward API Service: A service that manages user rewards and loyalty programs.

## Setup and Configuration

To run the microservices application locally, follow these steps:

1. Make sure you have installed .NET Core 7 on your system.
2. Clone this repository to your local machine.
3. Open each service's project folder separately and restore the dependencies using the following command:
   
   

   dotnet restore
   
Copy code


4. Configure the connection strings and other settings in the `appsettings.json` or `appsettings.Development.json` file of each service.
5. Build each service using the following command:

   

   dotnet build
   
Copy code


6. Run each service using the following command:

   

   dotnet run
   
Copy code


   NOTE: Make sure to run each service in a separate terminal window or using a container orchestration tool like Docker Compose.

## Usage

Once the microservices are up and running, you can start sending requests to the API Gateway service, which will forward the requests to the appropriate microservices based on the route configuration defined in Ocelot.

The API Gateway service will handle authentication and authorization by interacting with the Auth API service. It will also communicate with other microservices to perform various operations like managing coupons, products, shopping carts, orders, email notifications, and payment processing.

Feel free to explore the individual services' APIs and functionalities for more details on each microservice.

## Contributing

If you want to contribute to this microservices application, you can follow the steps below:

1. Fork this repository to your own GitHub account.
2. Clone the forked repository to your local machine.
3. Create a new branch for your changes.
4. Make the necessary changes and test your modifications.
5. Commit and push your changes to your forked repository.
6. Submit a pull request to the original repository.

Please ensure that your contributions align with the coding standards and guidelines followed in this project.


#Ahmed_Abd_Elsalam
