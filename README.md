
# Microservices Project

This project is built using a Microservices Architecture to manage product and order-related services independently. It enables both asynchronous and synchronous communication between services, providing flexibility and efficiency in operations.

## Modules

### 1. Product Services
The **Product Services** module is responsible for handling all operations related to products. It includes functionality for:
- Fetching all products
- Creating new products
- Additional product-related operations as needed

### 2. Order Services
The **Order Services** module manages the logic behind ordering products. This module handles:
- Placing orders
- Order management and processing
- Additional order-related operations as needed

## Communication Between Services

The project utilizes two types of communication between services:

- **Asynchronous Communication**: Implemented via **Kafka** to decouple services and allow non-blocking message-based communication. This is used for scenarios where real-time response isn’t necessary, and events can be processed in the background.
- **Synchronous Communication**: Implemented using **API calls** between services. This is used when immediate responses are needed, ensuring real-time interaction between services.

## API Gateway

An **API Gateway** has been implemented using **Ocelot** to provide a single entry point for all incoming requests. 

## Technologies Used

- **SQL Server**: Used as the relational database for storing product and order data.
- **Dapper**: Lightweight ORM for handling database connections and operations in a fast and efficient manner.
- **Kafka**: Serves as the message broker for asynchronous communication between services. It allows for reliable, decoupled message-based data exchanges.
- **Docker**: Used to set up and manage the server environment, particularly for the Kafka message broker, ensuring a consistent environment across different development and deployment stages.
- **Ocelot**: Used as the API Gateway to aggregate and route requests to the respective microservices.

## Architecture Overview

The project is divided into modular services, each responsible for a specific business domain. Communication between these services is handled via Kafka for asynchronous tasks and API calls for synchronous tasks, enabling flexibility and scalability.

Each service operates independently, leveraging the benefits of microservices, such as:

- **Scalability**: Each module can be scaled individually based on demand.
- **Resilience**: Independent services ensure that issues in one module don’t directly impact others.
- **Flexible Deployment**: Services can be deployed, updated, or rolled back without affecting the entire application.
- **Centralized API Management**: Ocelot API Gateway provides a unified entry point, simplifying the interaction between clients and microservices.
