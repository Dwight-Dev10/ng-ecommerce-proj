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
import { SignInParams, SignUpParams, User } from './models/user';
import { Router } from '@angular/router';
import { Order } from './models/orders';
import { withStorageSync } from '@angular-architects/ngrx-toolkit';
import { AddReviewParams, UserReview } from './models/user-review';
import { ProductService } from './services/ProductService';

export type EcommerceState = {
  products: Product[];
  category: string; // List of categories
  categories: string[]; // Currently selected category
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
  withState({
    products: [],
    category: 'all',
    categories: [],

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
          patchState(store,{ searchTerm: '', searchedResults: [], loading:false} );
          return;
        }
        patchState(store, { searchTerm: term , loading: true });

        productService.getSearchForProduct(term).subscribe({
          next: (results) => {
            patchState(store, {searchedResults: results,  loading: false });
          console.log(results)

          },
          error: (err) => {
            console.error('Unable to load products. Fun: SearchProducts', err);
            patchState(store, { searchedResults: [], loading: false });
            toaster.error('Failed to load search product.');
          },
        });
      },

      loadCategories: () => {
        productService.getCategories().subscribe({
          next: (categories) => {
            patchState(store, { categories });
          },
          error: (err) => {
            console.error('Unable to load categories', err);
            toaster.error('Failed to load categories.');
          },
        });
      },

      setCategory: (category: string) => {
        patchState(store, {
          category: category.trim().toLowerCase(),
          
        });
      },

      
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
        patchState(store, {
          user: {
            id: '1',
            name: 'John Doe',
            email,
            imageUrl: 'https://randomuser.me/api/portraits/men/1.jpg',
          },
        });
        matDialog.getDialogById(dialogId)?.close();
        if (checkout) {
          router.navigate(['/checkout']);
        }
      },

      signOut: () => {
        patchState(store, { user: undefined });
      },

      signUp: ({ email, password, name, checkout, dialogId }: SignUpParams) => {
        patchState(store, {
          user: {
            id: '1',
            name: 'John Doe',
            email,
            imageUrl: 'https://randomuser.me/api/portraits/men/1.jpg',
          },
        });
        matDialog.getDialogById(dialogId)?.close();
        if (checkout) {
          router.navigate(['/checkout']);
        }
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
