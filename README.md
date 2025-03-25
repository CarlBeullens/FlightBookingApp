# Flight Booking System

![CI](https://github.com/CarlBeullens/FlightBookingApp/actions/workflows/01-build-docker-images.yml/badge.svg)
![CD](https://github.com/CarlBeullens/FlightBookingApp/actions/workflows/02-deploy-to-azure.yml/badge.svg)

> **Note:** This project is currently under construction.

A basic flight booking system built with .NET 8 using a microservice architecture.

## Project Overview

This project aims to create a simple flight booking system with currently two main services:

- **Flight Service**: Manages flight information and availability
- **Booking Service**: Handles user bookings

Both services are containerized using Docker and communicate via REST APIs.

## Current Status

The project is in early development with the following components in progress:

- Basic service structure and communication
- Entity models for flights and bookings
- Integration with external APIs
- Docker configuration
- CI/CD pipeline with GitHub Actions

## Tech Stack

- .NET 8
- Entity Framework Core
- Docker
- Azure Container Apps
- Azure SQL Server
