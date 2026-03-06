# Event Management System API

A backend RESTful API for managing events, registrations, and payments.

This project was built using ASP.NET Core Web API and follows a clean architecture approach using services, repositories, and the Unit Of Work pattern. The system allows organizers to create and manage events, attendees to register for events, and supports secure payments using Stripe.

The goal of this project was to practice building a real-world backend system with authentication, payments, email notifications, and role-based authorization.

---

## Features

### Authentication & Authorization
- User registration
- Secure user login
- JWT authentication
- Role-based authorization (Admin, Organizer, Attendee)
- Admin can change user roles

### Event Management
- Create events
- Update event details
- Delete events
- Browse all available events
- Pagination for event listing
- Event capacity management
- Event status (Scheduled, Completed, Cancelled)

### Event Registration
- Register for events
- Cancel event registration
- View user's registered events
- Prevent duplicate registrations
- Prevent organizer from registering in their own event
- Capacity validation to avoid overbooking

### Payments
- Stripe payment integration
- Create payment intent for paid events
- Update payment amount if needed
- Webhook handling for payment confirmation
- Registration payment status updated automatically

### Email Notifications
- Email confirmation after successful event registration
- Event reminder emails before the event

### Admin Features
- View all users
- View user roles
- Change user roles (Admin / Organizer / Attendee)

---

## Technologies Used

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT Authentication
- Stripe Payment API
- AutoMapper
- Repository Pattern
- Unit Of Work Pattern
- Background Services
- SMTP Email Service
- Swagger (API Documentation)

---

## Project Architecture

The project follows a layered architecture to keep the code organized and maintainable.

### API Layer
Contains controllers responsible for handling HTTP requests and responses.

### Core Layer
Contains:
- Entities
- DTOs
- Interfaces
- Specifications
- Helper classes

### Repository Layer
Responsible for:
- Database access
- Entity Framework Core configurations
- Repository implementations

### Services Layer
Contains business logic such as:
- Event management
- User management
- Registration logic
- Payment handling
- Email notifications

---

## Main API Endpoints

### Authentication

POST `/api/account/register`  
Register a new user.

POST `/api/account/login`  
Login and receive JWT token.

---

### Events

GET `/api/events`  
Get all events with pagination.

GET `/api/events/{id}`  
Get event details.

POST `/api/events`  
Create a new event (Organizer / Admin).

PUT `/api/events/{id}`  
Update an event (Owner Organizer or Admin).

DELETE `/api/events/{id}`  
Delete an event (Owner Organizer or Admin).

---

### Registrations

POST `/api/registrations/{eventId}`  
Register for an event.

DELETE `/api/registrations/{eventId}`  
Cancel registration.

GET `/api/registrations/my`  
Get all registrations for the logged-in user.

---

### Payments

POST `/api/payments/{registrationId}/create-intent`  
Create Stripe payment intent for event registration.

---

### Stripe Webhook

POST `/api/stripe/webhook`  
Handles Stripe events such as:

- payment_intent.succeeded
- payment_intent.payment_failed

Updates registration payment status automatically.

---

### Admin

GET `/api/admin/users`  
Get all users with their roles.

PUT `/api/admin/change-role`  
Change a user's role.

---

## API Documentation

Swagger UI is available when running the project.

Example:

