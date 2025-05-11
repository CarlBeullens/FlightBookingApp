# Flight Booking System

![CI](https://github.com/CarlBeullens/FlightBookingApp/actions/workflows/01-build-docker-images.yml/badge.svg)
![CD](https://github.com/CarlBeullens/FlightBookingApp/actions/workflows/02-deploy-to-azure.yml/badge.svg)

> **Note:** This project is currently under construction.

A distributed flight booking system built with .NET 8 using microservices architecture. 
The system is designed to handle flight reservations, booking management, payments, and security through multiple interconnected services.

## Architecture Overview

The system consists of the following microservices:

- **Flight Service**: Manages flight information, availability, and seat inventory
- **Booking Service**: Handles flight bookings, passenger information, and booking lifecycle
- **Payment Service**: Processes payments and manages payment status
- **Security Service**: Handles user authentication and authorization with JWT tokens
- **Gateway**: API Gateway using YARP for routing and authentication

Services communicate via REST APIs and Azure Service Bus for asynchronous messaging.

## Tech Stack

- **Framework**: .NET 8
- **Database**: Azure SQL Server (with Entity Framework Core)
- **Messaging**: Azure Service Bus
- **Caching**: Redis
- **Authentication**: JWT Bearer Authentication
- **API Gateway**: YARP (Yet Another Reverse Proxy)
- **API Documentation**: Swagger/OpenAPI
- **External APIs**: Amadeus API for travel data
- **Containerization**: Docker
- **CI/CD**: GitHub Actions
- **Hosting**: Azure Container Apps

## Architecture Patterns

- **Microservices Architecture**: Independent services with clear boundaries
- **Event-Driven Communication**: Asynchronous messaging between services
- **API Gateway Pattern**: Centralized entry point for all services
