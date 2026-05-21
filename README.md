# SharedPool API

> This project is currently under active development.
> New features and architectural improvements are continuously being added.

## Overview

**SharedPool** is a backend service designed to manage shared expenses and debts between groups of people such as roommates, travel groups, or friends.

The project focuses on implementing scalable backend architecture, clean business logic separation, and real-world financial calculations using modern software engineering practices.

One of the main goals of this project is to simulate how production-grade expense management systems handle:

* complex business rules
* financial consistency
* validation pipelines
* transaction management
* extensible splitting algorithms

---

# Architecture & Technologies

The project follows **Clean Architecture** principles with strict dependency isolation using the **Dependency Inversion Principle**.

## Tech Stack

* **Framework:** .NET 8
* **Architecture:** Clean Architecture
* **Pattern:** CQRS with MediatR
* **Database:** PostgreSQL
* **ORM:** Entity Framework Core
* **Validation:** FluentValidation
* **Containerization:** Docker & Docker Compose

---

# Design Patterns & Engineering Concepts

## Strategy Pattern

Expense splitting algorithms are implemented using the **Strategy Pattern** to comply with the **Open/Closed Principle (SOLID)**.

This allows new splitting methods to be added without modifying existing business logic.

Supported strategies:

* Equal Split
* Percentage Split
* Exact Amount Split

---

## Repository & Unit of Work

Database operations are abstracted using:

* Repository Pattern
* Unit of Work Pattern

This ensures:

* transaction consistency
* cleaner data access logic
* improved testability

---

## Global Exception Handling

The project includes:

* Custom Domain Exceptions
* Global Exception Handling (`IExceptionHandler`)
* Centralized error responses

This helps create predictable and maintainable API behavior.

---

## Validation Pipeline

Request validation is handled using:

* FluentValidation
* MediatR Pipeline Behaviors

This keeps controllers clean and moves validation logic into the application layer.

---

## Event-Driven Architecture (EDA) & Asynchronous Balance Management

To maintain minimum **Response Time** during high-traffic periods and reduce **Coupling** between services, the project implements an **Event-Driven Architecture (EDA)**.

Calculations such as "who owes how much to whom" are not performed via traditional (synchronous) methods at the API layer. Instead, an asynchronous communication network (Pub/Sub) has been established using **RabbitMQ and MassTransit**.

* **Publisher:** When a user creates a new expense, once the database transaction is successfully completed, the system triggers an `ExpenseCreatedEvent` to RabbitMQ and immediately returns a `201 Created` response to the user.
* **Consumer:** An independent background listener (`ExpenseCreatedEventConsumer`) captures this event from the queue. It reads the split data within the event and updates the `UserBalances` table, which is optimized for read/query operations.

**Benefits of This Architecture:**
1.  **Isolation and Performance:** The balance calculation load is offloaded from the main API flow to background services.
2.  **Scalability:** The system can be extended by simply adding new Consumers (e.g., for sending Notifications) without modifying existing code (**Open/Closed Principle**).
3.  **Eventual Consistency:** System components operate independently, and data reaches final consistency within milliseconds.

---

# Core Features

## User & Group Management

* Create users
* Create expense groups
* Add members to groups

Strict domain rules are enforced to preserve data consistency.

Example:

* A user who is not part of a group cannot participate in that group's expenses.

---

# Flexible Expense Splitting

Expenses can be split using multiple strategies.

The system is designed to support extensible business rules without large `if/else` chains.

## Equal Split

Splits the expense equally between all participants.

### Rounding Distribution Problem

Financial calculations introduce rounding problems.

Example:

```text
100 / 3 = 33.33
33.33 + 33.33 + 33.33 = 99.99
```

To preserve financial consistency:

* all values are rounded to 2 decimal places
* the remaining difference is automatically distributed to one participant

This guarantees that:

* the distributed total always matches the original expense amount
* no money is lost because of rounding issues

This behavior is similar to how real-world expense management systems handle financial precision.

---

## Percentage Split

Splits the expense according to user-defined percentages.

Validation rules ensure:

* percentages must total 100%

---

## Exact Amount Split

Allows participants to enter exact amounts manually.

Validation rules ensure:

* total entered amounts must equal the original expense amount

---

# Project Structure

```text
SharedPool
├── SharedPool.API
├── SharedPool.Application
├── SharedPool.Domain
├── SharedPool.Infrastructure
```

### Layer Responsibilities

| Layer          | Responsibility                            |
| -------------- | ----------------------------------------- |
| API            | Endpoints & HTTP configuration            |
| Application    | CQRS, validation, business workflows      |
| Domain         | Core business rules & entities            |
| Infrastructure | Database, repositories, external services |

---

# Example Request

## Create Expense

```http
POST /api/expenses
```

```json
{
  "groupId": "group-guid",
  "paidByUserId": "user-guid",
  "amount": 100,
  "splitType": "Equal",
  "participants": [
    {
      "userId": "user-1"
    },
    {
      "userId": "user-2"
    },
    {
      "userId": "user-3"
    }
  ]
}
```

---

# Running the Project

## Requirements

* Docker
* Docker Compose

---

## Clone the Repository

```bash
git clone https://github.com/utkucakarr/SharedPool
cd SharedPool
```

---

## Run with Docker

```bash
docker-compose up --build -d
```

After the containers are started, the API will be available at:

```text
http://localhost:8080
```

You can test the endpoints using:

* Postman
* Swagger

---

# Author

Developed by [Utku Çakar](https://github.com/utkucakarr/SharedPool)

Feel free to contribute, give feedback, or explore the project.
