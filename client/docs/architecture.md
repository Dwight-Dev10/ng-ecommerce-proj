# NgEcommerce Architecture

## Overview
NgEcommerce is a modern Angular 21 storefront built with standalone components, Angular Signals, Material UI, and server-side rendering. The app follows a modular storefront architecture where route-driven pages, stateful services, and component-level actions are separated cleanly.

## Tech Stack
- Angular 21
- Standalone components
- Angular Signals and `@ngrx/signals`
- Angular Material
- RxJS and `HttpClient`
- SSR support via `@angular/ssr`
- Tailwind-style utility classes for layout and spacing

## Application Structure
- `src/app/app.ts`
  - Root application shell.
  - Hosts the global layout and router outlet.
- `src/app/app.routes.ts`
  - Declares the route structure for catalog, detail, cart, checkout, wishlist, and success screens.
- `src/app/app.config.ts`
  - Registers app-level providers such as router configuration, Material setup, and toast utilities.
- `src/app/app.config.server.ts`
  - Adds server-side rendering configuration for the Angular app.

## Routing Model
The application uses route-driven page composition:

- `/products/:category` — product catalog filtered by category
- `/product/:productId` — product detail page
- `/wishlist` — saved products
- `/cart` — shopping cart
- `/checkout` — order placement flow
- `/order-success` — confirmation page

The default route redirects to `/products/all`.

## State Management
The central application state is maintained in `src/app/ecommerce-store.ts` using `signalStore` from `@ngrx/signals`.

### State slices
- `products`
- `category`
- `wishlistItems`
- `cartItems`
- `user`
- `selectedProductId`
- `loading`
- `writeReview`

### Derived values
- `filteredProducts`
- `wishListCount`
- `cartCount`
- `selectedProduct`

### Store responsibilities
The signal store manages product loading, category filtering, cart updates, wishlist interactions, checkout, sign-in/sign-up state, and review submission logic. It uses immutable array updates via `immer` to keep updates predictable and easy to reason about.

## Service Architecture
The application separates transport concerns from UI logic through Angular services.

### ProductService
`src/app/services/ProductService.ts` is the main API client. It injects `HttpClient` and exposes product CRUD and search operations against the backend environment URL.

The base API URL is configured in `src/environments/environment.development.ts`:

```ts
apiUrl: 'http://localhost:5296/api'
```

This service is responsible for:
- fetching the product catalog
- retrieving a product by name or ID
- fetching by category
- searching by keyword
- creating, updating, and deleting products

## Component Design
The application is built using standalone components instead of NgModules. This keeps modules smaller and makes route-based component loading straightforward.

### Key components
- `src/app/layout/header/header.ts` — top navigation and app-level actions
- `src/app/components/product-card/product-card.ts` — product summary card
- `src/app/components/toggle-wishlist-button/toggle-wishlist-button.ts` — add/remove wishlist control
- `src/app/components/summarize-order/summarize-order.ts` — order total panel
- `src/app/components/qty-selector/qty-selector.ts` — quantity control
- `src/app/components/sign-in-dialog/sign-in-dialog.ts` — user sign-in UI
- `src/app/components/sign-up-dialog/sign-up-dialog.ts` — registration UI

## Models and Domain Objects
The app uses typed model files to define domain objects and payloads:

- `src/app/models/product.ts` — product entity and review data
- `src/app/models/cart.ts` — cart item model
- `src/app/models/user.ts` — auth and user profile types
- `src/app/models/orders.ts` — order object and checkout payload
- `src/app/models/user-review.ts` — review entity type

## .NET API Integration
The frontend is designed to work with a .NET API backend at `http://localhost:5296/api`. Product data and possibly future auth/order flows are intended to be driven by the API rather than static local arrays.

This keeps the storefront architecture extensible:
- UI remains Angular-specific
- domain contracts are shared through typed TypeScript models
- API integration is centralized in a service layer
- the store retains client-side state for cart and UX behaviors

## UI and Styling
- Angular Material provides dialogs, side navigation, lists, and progress indicators.
- Utility-based styling enables a lightweight responsive storefront experience.
- Product cards, category panels, order summaries, and checkout sections are all componentized and reusable.

## SSR and Performance
The project includes SSR support with Angular’s server platform and hydration configuration. This gives the application a better foundation for SEO, performance, and future server-rendered improvements while still preserving a client-side storefront experience.
