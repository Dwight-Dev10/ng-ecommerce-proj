# NgEcommerce Features

## Product Catalog
- The storefront provides a category-based product grid that loads via route parameters such as `/products/electronics`.
- Categories include `all`, `Electronics`, `Clothing`, `Home & Kitchen`, `Fitness`, `Accessories`, and `Home & Office`.
- Each product card displays:
  - product image
  - product name
  - price
  - rating and review count
  - stock status
  - quick add-to-cart action
  - wishlist toggle
- Product listing is driven by the `EcommerceStore` and the backend product service.

## Search and Filtering
- Users can search products by name or keyword through the `ProductService` search flow.
- Category navigation is handled through the sidebar and route-based filtering.
- Search and category results are surfaced in the same catalog page for a consistent shopping experience.

## Product Details
- Product detail pages render a complete product overview with:
  - main product image
  - detailed description
  - pricing and availability
  - category indication
  - customer ratings and review count
  - review list
  - add-to-cart quantity selector
  - wishlist action
- The detail view also supports a review-writing workflow for authenticated users.

## Shopping Cart Management
- The cart supports:
  - adding products from catalog or detail views
  - updating item quantities
  - removing items
  - moving items into or out of the wishlist
  - viewing an order summary with subtotal, tax, and total
- The cart state is stored in the Angular signal store and updated with immutable changes.

## Wishlist
- Users can save products for later using the wishlist toggle button.
- The wishlist page shows all saved items and provides actions to:
  - remove an item
  - clear the entire wishlist
  - move items into the cart
- This feature is designed to improve browsing and product comparison without forcing immediate purchase.

## Checkout Experience
- The checkout flow includes shipping information, payment form fields, and the order summary panel.
- The app requires a signed-in user before placing an order.
- If a customer is not logged in, the sign-in dialog appears automatically.
- After order placement, the cart is cleared and the user is redirected to the order success page.

## Authentication
- The app includes sign-in and sign-up dialogs.
- Auth state is stored in the central store and used to gate checkout and review actions.
- Signed-in users can create product reviews and complete purchases.
- Sign-out clears the current user session and resets access to protected actions.

## Review System
- Product reviews are part of the product model and displayed on the detail page.
- Users can write a review with a title, comment, and star rating.
- Review data updates the product’s local average rating and review count in the store.

## New Product and API Integration Features
- The frontend is now aligned with an ASP.NET Core API backend at `http://localhost:5296/api`.
- Product fetching and product search are implemented through dedicated HTTP service methods.
- The architecture supports future expansion for:
  - order submission over API
  - real authentication
  - product management admin features
  - persistent inventory and review data

## UI and UX Improvements
- The storefront uses Angular Material for a polished interface.
- Toast notifications confirm actions such as cart updates, wishlist changes, sign-in, and order success.
- Headless, route-driven pages keep the experience responsive and easy to extend.
- Server-side rendering support improves the project’s production readiness and scalability.

## Current State
The application is now positioned as a modern e-commerce frontend with a .NET-ready API integration layer and a complete storefront flow from browsing to checkout. The UI is fully functional, and the Angular service layer is ready to integrate with a live ASP.NET backend.
