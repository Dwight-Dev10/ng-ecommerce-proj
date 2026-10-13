import { Component, computed, inject, input } from '@angular/core';
import { MatIcon } from "@angular/material/icon";
import { Product } from '../../models/product';
import { EcommerceStore } from '../../ecommerce-store';
import { MatIconButton } from '@angular/material/button';

@Component({
  selector: 'app-toggle-wishlist-button',
  imports: [MatIcon, MatIconButton],
  template: ` 
  <button 
      [class]="isInWishlist() ? '!text-red-500' : '!text-gray-400'"
      matIconButton (click)="toggleWishList(product())"
      >
        <mat-icon>{{isInWishlist() ? 'favorite' : 'favorite_border'}}</mat-icon>
    </button>`,
styles: ``,
})
export class ToggleWishlistButton {

  product = input.required<Product>(); // Input property to receive the product for which the wishlist button is rendered

  store = inject(EcommerceStore); // Injecting the EcommerceStore for state management

  isInWishlist =  computed(() => this.store.wishlistItems().find(item => item.id === this.product().id)); // Computed property to check if the product is in the wishlist

  toggleWishList(product: Product) {
    // Implementation for toggling wishlist
    if(this.isInWishlist()){
      // remove item
      this.store.removeFromWishlist(product);
    } else {
      // add item
      this.store.addToWishlist(product);
    }

  }
}
