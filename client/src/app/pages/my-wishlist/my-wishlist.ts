import { Component, inject } from '@angular/core';
import { BackButton } from "../../components/back-button/back-button";
import { EcommerceStore } from '../../ecommerce-store';
import { ProductCard } from "../../components/product-card/product-card";
import { MatIcon } from "@angular/material/icon";
import { EmptyWishlist } from "./empty-wishlist/empty-wishlist";
import { MatButton, MatIconButton } from '@angular/material/button';

@Component({
  selector: 'app-my-wishlist',
  imports: [BackButton, ProductCard, MatIcon, MatButton, MatIconButton, EmptyWishlist],
  template: ` 
    <div class="mx-auto max-w-[1200] py-6 px-4">
      <app-back-button navigateTo="/products/all">
        Continue Shopping
      </app-back-button>

      @if (store.wishListCount() > 0) {
        <div class="flex justify-between items-center mb-6">
          <h1 class="text-2xl font-bold">My Wishlist</h1>
          <span class="text-gray-500 text-xl">{{ store.wishListCount() }} items</span>
        </div>

        <div class="responsive-grid">
          @for (product of store.wishlistItems(); track product.id) {
            <app-product-card [product]="product">
                <button 
                class="!absolute top-3 right-3 z-10 !bg-white shadow-md transition-all duration-200 hover:scale-110 hover:shadow-lg"
                matIconButton (click)="store.removeFromWishlist(product)"
                >
                  <mat-icon>delete</mat-icon>
            </button>
            </app-product-card>
          }
        </div>
        <div class="mt-8 flex justify-center">
          <button matButton="filled" class="danger" (click)="store.clearWishlist()">
            Clear Wishlist
          </button>
        </div>
      } @else {
        <app-empty-wishlist />
      }
    </div>
  
  
  `,
  styles: ``,
})
export default class MyWishlist {
  store = inject(EcommerceStore);
}
