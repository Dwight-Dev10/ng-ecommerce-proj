# Angular E-Commerce Client

A full-stack e-commerce learning project built with **Angular 21 and TypeScript**, with a focus on modern frontend development, application architecture, state management, and backend API integration.

The goal is to build a realistic application while developing a deeper understanding of how software systems are designed, implemented, tested, and prepared for deployment.

## Table of Contents

- [Overview](#overview)
- [Motivation](#motivation)
- [Technology Stack](#technology-stack)
- [Current Features](#current-features)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Development Commands](#development-commands)
- [Engineering Focus](#engineering-focus)
- [Roadmap](#roadmap)
- [Project Status](#project-status)

## Overview

This repository contains the Angular client for an e-commerce application that integrates with an ASP.NET Core Web API.

The application provides a foundation for common shopping experiences, including browsing products, filtering by category, searching for products, managing a shopping cart, and maintaining a wishlist.

Beyond implementing features, this project is an opportunity to explore the engineering decisions that make applications maintainable, responsive, and scalable as they grow.

## Motivation

I created this project to become a stronger software engineer and gain practical experience understanding how real-world applications are designed, built, and deployed.

Rather than focusing exclusively on getting features working, I want to understand why particular architectural decisions are made, how frontend and backend responsibilities should be separated, and how applications can be improved through thoughtful engineering.

Key learning objectives include:

- Applying modern Angular development patterns.
- Building maintainable, component-based application architecture.
- Managing shared state and derived data with Angular Signals and NgRx Signal Store.
- Integrating frontend applications with backend APIs.
- Understanding asynchronous programming and error handling.
- Exploring application performance, rendering strategies, and user experience.
- Preparing applications for cloud deployment and production-oriented environments.

## Technology Stack

| Technology | Purpose |
|---|---|
| Angular 21 | Frontend framework |
| TypeScript | Type-safe application development |
| Angular Material | UI components and interface elements |
| Angular Signals | Reactive state and derived values |
| NgRx Signal Store | Shared application state management |
| RxJS | Asynchronous operations and API data streams |
| Angular Router | Client-side navigation |
| Angular `HttpClient` | Backend API communication |
| ASP.NET Core | Backend Web API |
| Azure | Planned cloud hosting and infrastructure integration |

## Current Features

The following features have been implemented or developed as part of the current application.

- **Product browsing:** Retrieve and display product data from the backend API.
- **Dynamic categories:** Load product categories from the backend rather than relying on a hard-coded category list.
- **Category filtering:** Filter products according to the selected category.
- **Product search:** Retrieve matching products from the API and display search results separately from the main product collection.
- **Shopping cart:** Manage cart state through the application's shared store.
- **Wishlist:** Support wishlist functionality through shared application state.
- **Product navigation:** Navigate between product listings and individual product detail routes.
- **Responsive product grid:** Display products in a grid-based layout.
- **Reactive state management:** Use signals and computed values to keep displayed data synchronized with application state.
- **Angular Material integration:** Use Material components for interface elements and navigation.

The application is actively evolving, and individual features may still require additional validation, testing, or refinement before being considered production-ready.

## Architecture

The client is organized around a component-based architecture that separates presentation, application state, and API communication.

```text
Angular Components
        |
        v
NgRx Signal Store
        |
        v
API Services
        |
        v
ASP.NET Core Web API
        |
        v
Backend Services and Database
```

### Architectural responsibilities

**Components**

Provide the user interface, respond to user interactions, and render application data.

**NgRx Signal Store**

Coordinates shared application state, including product data, selected categories, search results, cart items, and wishlist items. Computed signals derive the data needed by the interface.

**API services**

Encapsulate communication with the backend API using Angular's `HttpClient` and return asynchronous results through RxJS observables.

**Backend API**

Provides product and category data and handles backend responsibilities independently of the Angular client.

This separation helps reduce unnecessary coupling between UI components, state management, and backend communication.

### Reactive state management

The application uses Angular Signals and NgRx Signal Store to manage state and derive values reactively.

For example, product listings can be derived from the main product collection, active search results, and selected category. Keeping search results separate from the main product collection prevents a search operation from unnecessarily replacing the underlying product data.

This provides a practical environment for learning how reactive state, computed values, and asynchronous API requests interact.

## Project Structure

The exact structure may evolve as the application develops. At a high level, the client separates UI components, API services, and shared state.

```text
src/
└── app/
    ├── components/
    │   ├── header/
    │   ├── product-card/
    │   ├── product-grid/
    │   └── ...
    ├── services/
    │   └── ...
    ├── ecommerce-store.ts
    ├── app.routes.ts
    └── app.config.ts
```

This is a conceptual overview rather than a complete directory listing. Update it as necessary to reflect the repository's actual file organization.

## Getting Started

### Prerequisites

Install the following tools before running the application:

- [Node.js](https://nodejs.org/)
- npm
- [Angular CLI](https://angular.dev/tools/cli)
- [Git](https://git-scm.com/)
- The backend API, available locally or through a configured deployment

### 1. Clone the repository

```bash
git clone <your-repository-url>
cd <your-client-directory>
```

Replace the placeholders with the actual repository URL and directory name.

### 2. Install dependencies

```bash
npm install
```

### 3. Configure the backend API

The Angular client requires access to the ASP.NET Core API to retrieve product and category data.

For local development, the API has been configured to use:

```text
http://localhost:5296/api
```

Ensure the backend is running and that the frontend's API configuration matches the address where it is hosted.

For deployment, use environment-appropriate configuration and avoid committing secrets or credentials to source control.

### 4. Start the development server

```bash
ng serve
```

Open the local URL displayed in the terminal, typically:

```text
http://localhost:4200
```

The development server rebuilds the application when source files change.

## Development Commands

Run the following commands from the project directory.

| Command | Description |
|---|---|
| `npm install` | Install project dependencies |
| `ng serve` | Start the development server |
| `ng build` | Build the application |
| `ng test` | Run the configured test suite |
| `ng generate component <name>` | Generate a component |
| `ng generate service <name>` | Generate a service |

The availability and behavior of test commands depend on the project's configured testing tools and npm scripts.

## Engineering Focus

This project is intended to explore practical software engineering concepts through implementation and iteration.

### Maintainability

Organize code around clear responsibilities and avoid placing unrelated API, state-management, and presentation logic in the same component.

### State management

Use reactive state and computed signals to derive UI data instead of manually synchronizing multiple collections. Handle loading, success, empty-result, and error states explicitly.

### API integration

Keep HTTP communication within services, handle failed requests appropriately, and make the frontend resilient to unexpected API responses.

### Performance

Investigate rendering behavior, unnecessary recomputation, efficient data retrieval, and the trade-offs involved in client-side rendering and server-side rendering.

### Testing and reliability

Expand automated test coverage, validate important user flows, and make changes with regression prevention in mind.

### Cloud readiness

Prepare the client for deployment alongside a cloud-hosted backend, with environment-specific configuration and appropriate handling of API endpoints.

## Roadmap

The following items represent planned or ongoing work, not a claim that these capabilities are already complete.

- [ ] Expand automated unit and integration test coverage.
- [ ] Improve API error handling and loading-state consistency.
- [ ] Refine search behavior and product filtering.
- [ ] Improve accessibility and responsive design.
- [ ] Evaluate server-side rendering and prerendering where beneficial.
- [ ] Integrate product images with Azure Blob Storage through the backend.
- [ ] Prepare the Angular client and ASP.NET Core API for cloud deployment.
- [ ] Review performance and identify optimization opportunities.
- [ ] Document architectural decisions and trade-offs.
- [ ] Explore a second backend implementation using Java, Spring Boot, and AWS as part of the broader full-stack learning project.

## Project Status

**Actively developing**

This project is continuously evolving as new features are implemented and existing architectural decisions are reviewed. The current priority is to build a maintainable application while developing a deeper understanding of modern frontend and full-stack engineering.

## License

No license has been specified yet. Add a `LICENSE` file if you intend to distribute the project under particular terms.
