import { computed, inject } from '@angular/core';
import { Product } from './models/product';
import {
  patchState,
  signalMethod,
  signalStore,
  withComputed,
  withMethods,
  withState,
} from '@ngrx/signals';
import { produce } from 'immer';
import { Toaster } from './services/toaster';
import { CartItem } from './models/cart';
import { MatDialog } from '@angular/material/dialog';
import SignInDialog from './components/sign-in-dialog/sign-in-dialog';
import { SignInParams, SignUpParams, User } from './models/user/user';
import { Router } from '@angular/router';
import { Order } from './models/orders';
import { AddReviewParams, UserReview } from './models/user/user-review';
import { ProductService } from './services/ProductService';
import { AuthService } from './services/auth.service';
import { AuthenticatedUser } from './models/auth/authenticated-user.model';
import { switchMap } from 'rxjs';

export type EcommerceState = {
  products: Product[];
  category: string; // List of categories
  // categories: string[]; // Currently selected category
  wishlistItems: Product[];
  cartItems: CartItem[];
  user: User | undefined;

  loading: boolean;
  selectedProductId: string | undefined;

  searchedResults: Product[],
  searchTerm: string;

  writeReview: boolean;
};

/* `EcommerceStore` is a signal store in Angular using `@ngrx/signals` library. It defines
the state of an e-commerce application including products, category, and wishlist
items. It initializes the state with a list of products and provides computed
properties for filtered products based on the selected  category and the count of items
in the wishlist. */
export const EcommerceStore = signalStore(
  {
    providedIn: 'root', //primary way to define a service in Angular that is available globally throughout your application
  },
  withState({ //  initial state object.
    products: [],
    category: 'all',
    // categories: [],

    wishlistItems: [],
    cartItems: [],
    user: undefined,

    searchedResults: [],
    searchTerm: '',

    loading: false,
    selectedProductId: undefined,
    writeReview: false,
  } as EcommerceState),

  // withStorageSync({
  //   key: 'modern-store',
  //   select: ({wishlistItems, cartItems, user}) => ({wishlistItems, cartItems, user }),
  //   }),

  withComputed(({ products, category, wishlistItems, cartItems, selectedProductId, searchTerm, searchedResults }) => ({
    // categories: computed(() => categories()),

    displayProducts: computed(() => {
      const term = searchTerm().trim();
      const selectedCategory = category().trim().toLowerCase();

      const sourceProduct = term ? searchedResults() : products();

      if (selectedCategory === 'all') {
        return sourceProduct;
      }

      return sourceProduct.filter((product) => product.category.trim().toLowerCase() === selectedCategory);
    }),

    wishListCount: computed(() => wishlistItems().length),
    cartCount: computed(() => cartItems().reduce((total, item) => total + item.quantity, 0)),
    selectedProduct: computed(() => products().find((p) => p.id === selectedProductId())),


  })),

  withMethods(
    (
      store,
      toaster = inject(Toaster),
      matDialog = inject(MatDialog),
      router = inject(Router),
      productService = inject(ProductService),
      authService = inject(AuthService),
    ) => ({
      loadProducts: () => {
        productService.getProducts().subscribe({
          next: (products) => {
            patchState(store, { products });
          },
          error: (err) => {
            console.error('Unable to load products', err);
            toaster.error('Failed to load products.');
          },
        });
      },

      searchProducts(searchTerm: string) {
        const term = searchTerm.trim();

        if (!term) {
          patchState(store, { searchTerm: '', searchedResults: [], loading: false });
          return;
        }
        patchState(store, { searchTerm: term, loading: true });

        productService.getSearchForProduct(term).subscribe({
          next: (results) => {
            patchState(store, { searchedResults: results, loading: false });
            console.log(results)

          },
          error: (err) => {
            console.error('Unable to load products. Fun: SearchProducts', err);
            patchState(store, { searchedResults: [], loading: false });
            toaster.error('Failed to load search product.');
          },
        });
      },


      setCategory: signalMethod<string>((category: string) => {
        patchState(store, { category });
      }),


      setProductId: signalMethod<string>((productId: string) => {
        patchState(store, { selectedProductId: productId });
      }),

      addToWishlist: (product: Product) => {
        const updatedWishlistItems = produce(store.wishlistItems(), (draft) => {
          if (!draft.find((p) => p.id === product.id)) {
            draft.push(product);
          }
        });
        patchState(store, { wishlistItems: updatedWishlistItems });
        toaster.success(`${product.name} added to wishlist!`);
      },

      removeFromWishlist: (product: Product) => {
        patchState(store, {
          wishlistItems: store.wishlistItems().filter((p) => p.id !== product.id),
        });
        toaster.error(`${product.name} removed from wishlist!`);
      },

      clearWishlist: () => {
        patchState(store, { wishlistItems: [] });
      },
      addToCart: (product: Product, quantity = 1) => {
        const existingCartItem = store
          .cartItems()
          .findIndex((item) => item.product.id === product.id);
        // Produce will give us an immutable update to the cart items array
        const updatedCartItems = produce(store.cartItems(), (draft) => {
          if (existingCartItem !== -1) {
            draft[existingCartItem].quantity += quantity;
          } else {
            draft.push({ product, quantity });
          }
        });

        patchState(store, { cartItems: updatedCartItems });
        toaster.success(
          existingCartItem === -1
            ? `${product.name} added to cart!`
            : `${product.name} product already in cart!`,
        );
      },

      setItemQuantity(params: { productId: string; quantity: number }) {
        const index = store.cartItems().findIndex((item) => item.product.id === params.productId);
        const update = produce(store.cartItems(), (draft) => {
          draft[index].quantity = params.quantity;
        });
        patchState(store, { cartItems: update });
      },

      addAllWishlistToCart: () => {
        const updatedCartItems = produce(store.cartItems(), (draft) => {
          store.wishlistItems().forEach((p) => {
            if (!draft.find((c) => c.product.id === p.id)) {
              draft.push({ product: p, quantity: 1 });
            }
          });
        });
        patchState(store, { cartItems: updatedCartItems, wishlistItems: [] });
      },

      moveToWishlist: (product: Product) => {
        const updatedCartItems = store.cartItems().filter((p) => p.product.id !== product.id);
        const updatedWishlistItems = produce(store.wishlistItems(), (draft) => {
          if (!draft.find((p) => p.id === product.id)) {
            draft.push(product);
          }
        });

        patchState(store, { cartItems: updatedCartItems, wishlistItems: updatedWishlistItems });
      },

      removeFromCart: (product: Product) => {
        patchState(store, {
          cartItems: store.cartItems().filter((p) => p.product.id !== product.id),
        });
      },

      proceedToCheckout: () => {
        if (!store.user()) {
          matDialog.open(SignInDialog, {
            disableClose: true,
            data: { checkout: true },
          });
          return;
        }
        router.navigate(['/checkout']);
      },

      placeOrder: async () => {
        patchState(store, { loading: true });

        const user = store.user();
        if (!user) {
          toaster.error('You must be signed in to place an order.');
          patchState(store, { loading: false });
          return;
        }

        const order: Order = {
          id: crypto.randomUUID(),
          userId: user.id,
          total: Math.round(
            store.cartItems().reduce((acc, item) => acc + item.quantity * item.product.price, 0),
          ),
          items: store.cartItems(),
          paymentStatus: 'success',
        };

        await new Promise((resolve) => setTimeout(resolve, 1000));

        patchState(store, { cartItems: [], loading: false });
        router.navigate(['order-success']);
        toaster.success('Order placed successfully!');
      },

      signIn: ({ email, password, checkout, dialogId }: SignInParams) => {
        patchState(store, { loading: true });

        authService.login({ email, password })
          .pipe(
            switchMap(() => authService.getMe())
          )
          .subscribe({
            next: (authenticatedUser) => {
              patchState(store, {
                user: {
                  id: authenticatedUser.userId,
                  name: authenticatedUser.email,
                  email: authenticatedUser.email,
                  imageUrl: '',
                },
                loading: false,
              });

              matDialog.getDialogById(dialogId)?.close();

              if (checkout) {
                router.navigate(['/checkout']);
              }
            },
            error: (err) => {
              console.error('Authentication flow failed', err);

              authService.clearToken();
              patchState(store, { loading: false });

              toaster.error('Unable to sign in. Check your credentials and try again.');
            },
          });
      },

      signOut: () => {
        patchState(store, { user: undefined });
      },

      //       Call AuthService.register().
      // Set loading while the request is in progress.
      // Close the sign-up dialog on success.
      // Open the sign-in dialog so the user can log in.
      // Display an error if registration fails.

      signUp: ({ name, email, password, confirmPassword, checkout, dialogId }: SignUpParams) => {
        patchState(store, { loading: true });

        authService.register({
          name,
          email,
          password,
          confirmPassword
        }).subscribe({
          next: () => {
            patchState(store, { loading: false });

            matDialog.getDialogById(dialogId)?.close();

            matDialog.open(SignInDialog, {
              data: { checkout }
            });

            toaster.success('Account created successfully. Please sign in.');
          },
          error: (err) => {
            console.error('Registration failed', err);

            patchState(store, { loading: false });

            toaster.error('Unable to create account. Please try again.');
          }
        });
      },

      showWriteReview: () => {
        patchState(store, { writeReview: true });
      },

      hideWriteReview: () => {
        patchState(store, { writeReview: false });
      },

      addReview: async ({ title, comment, rating }: AddReviewParams) => {
        patchState(store, {
          loading: true,
        });
        const productId = store.products().find((p) => p.id === store.selectedProductId());

        if (!productId) {
          toaster.error('Product not found.');
          patchState(store, { loading: false });
          return;
        }

        const newReview: UserReview = {
          id: crypto.randomUUID(),
          productId: productId.id,
          UserName: store.user()?.name || 'Anonymous',
          userImageUrl: store.user()?.imageUrl || '',
          rating,
          title,
          comment,
          reviewDate: new Date(),
        };

        const updatedProducts = produce(store.products(), (draft) => {
          const productIndex = draft.findIndex((p) => p.id === productId.id);
          draft[productIndex].reviews.push(newReview);
          draft[productIndex].rating =
            Math.round(
              (draft[productIndex].reviews.reduce((acc, r) => acc + r.rating, 0) /
                draft[productIndex].reviews.length) *
              10,
            ) / 10;
          draft[productIndex].reviewCount = draft[productIndex].reviews.length;
        });

        // await new Promise((resolve) => setTimeout(resolve, 1000));
        // patchState(store, {loading: false, products: updatedProducts, writeReview: false});
      },
    }),
  ),
);
