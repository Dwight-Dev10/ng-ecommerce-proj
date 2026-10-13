# NgEcommerce API Documentation

## Overview
The Angular storefront is configured to consume an ASP.NET Core Web API backend. The frontend base URL is defined in `src/environments/environment.development.ts` as:

```ts
apiUrl: 'http://localhost:5296/api'
```

This means the app expects the REST API to run locally on port `5296` during development.

## Current Integration Status
- Angular uses `HttpClient` through `ProductService` for product retrieval and search operations.
- The application still includes a client-side fallback store for local demo usage, but the API layer is now the primary integration point.
- `EcommerceStore.loadProducts()` calls the API as part of the storefront data flow.

## Service Layer
`src/app/services/ProductService.ts` is the main API abstraction for product operations.

### ProductService methods
- `getProducts(): Observable<Product[]>`
- `getProductsByName(name: string): Observable<Product>`
- `getProductsByCategory(cat: string): Observable<Product>`
- `getSearchForProduct(searchTerm: string): Observable<Product[]>`
- `createProduct(product: Product): Observable<Product>`
- `updateProduct(product: Product): Observable<Product>`
- `deleteProduct(productId: number): Observable<void>`

This service maps directly to REST endpoints under the `/api/products` resource.

## API Contract
The backend is expected to expose REST-style endpoints aligned with the Angular service layer.

### GET /api/products
Retrieves the entire catalog for the storefront.

Response:
```ts
[
  {
    id: 'p1',
    name: 'Wireless Bluetooth Headphones',
    description: 'High-quality over-ear headphones with noise cancellation and 20-hour battery life.',
    price: 129.99,
    imageUrl: 'https://picsum.photos/seed/headphones/400/300',
    rating: 4.5,
    reviewCount: 342,
    inStock: true,
    category: 'Electronics',
    reviews: [
      {
        id: 'r1-1',
        productId: 'p1',
        UserName: 'Ava M.',
        userImageUrl: 'https://randomuser.me/api/portraits/women/45.jpg',
        rating: 5,
        title: 'Fantastic sound quality',
        comment: 'These headphones are comfortable and the noise cancellation is amazing.',
        reviewDate: '2026-05-14T00:00:00.000Z'
      }
    ]
  }
]
```

### GET /api/products/{productId}
Returns a single product with the same schema as the catalog item.

### GET /api/products/category/{category}
Returns products filtered by category.

### GET /api/products/search?name={searchTerm}
Searches products by name or keyword.

### POST /api/products
Creates a new product entry.

Request body:
```ts
{
  id: 'p13',
  name: 'Portable SSD',
  description: 'Fast and reliable external drive for storage and backups.',
  price: 119.99,
  imageUrl: 'https://example.com/ssd.jpg',
  rating: 4.8,
  reviewCount: 18,
  inStock: true,
  category: 'Electronics',
  reviews: []
}
```

### PUT /api/products/{productId}
Updates an existing product.

### DELETE /api/products/{productId}
Removes a product from the catalog.

### POST /api/orders
Creates a new customer order.

Request body:
```ts
{
  userId: '1',
  items: [
    {
      product: {
        id: 'p1',
        name: 'Wireless Bluetooth Headphones',
        price: 129.99
      },
      quantity: 1
    }
  ],
  total: 129.99,
  paymentStatus: 'success'
}
```

Response:
```ts
{
  id: 'ord_123',
  userId: '1',
  total: 129.99,
  items: [],
  paymentStatus: 'success'
}
```

### POST /api/auth/signin
Authenticates a user.

Request body:
```ts
{
  email: 'john@example.com',
  password: 'Password123!'
}
```

Response:
```ts
{
  id: '1',
  name: 'John Doe',
  email: 'john@example.com',
  imageUrl: 'https://randomuser.me/api/portraits/men/1.jpg'
}
```

### POST /api/auth/signup
Registers a new user account.

## Shared Data Models

### Product
```ts
export type Product = {
  id: string;
  name: string;
  description: string;
  price: number;
  imageUrl: string;
  rating: number;
  reviewCount: number;
  inStock: boolean;
  category: string;
  reviews: UserReview[];
};
```

### CartItem
```ts
export type CartItem = {
  product: Product;
  quantity: number;
};
```

### User
```ts
export type User = {
  id: string;
  name: string;
  email: string;
  imageUrl: string;
};
```

### Order
```ts
export type Order = {
  id: string;
  userId: string;
  total: number;
  items: CartItem[];
  paymentStatus: 'success' | 'failure';
};
```

### UserReview
```ts
export type UserReview = {
  id: string;
  productId: string;
  UserName: string;
  userImageUrl: string;
  rating: number;
  title: string;
  comment: string;
  reviewDate: Date;
};
```

## Implementation Notes
- Product loading, catalog filtering, and search are intended to be powered by the ASP.NET Core API instead of static in-memory data.
- The `EcommerceStore` still manages local cart, wishlist, auth session, and review UI state in the client, while the API handles data persistence and retrieval.
- The Angular app is ready for a real backend contract and is already structured around HTTP-based service calls and typed frontend models.
