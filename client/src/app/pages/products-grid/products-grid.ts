import { Component, effect, inject, input } from '@angular/core';
import { Product } from '../../models/product';
import { ProductCard } from '../../components/product-card/product-card';
import { MatSidenav, MatSidenavContainer, MatSidenavContent } from '@angular/material/sidenav';
import { MatListItemTitle, MatNavList, MatListItem } from '@angular/material/list';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterLink } from '@angular/router';
import { TitleCasePipe } from '@angular/common';
import { EcommerceStore } from '../../ecommerce-store';
import { ToggleWishlistButton } from '../../components/toggle-wishlist-button/toggle-wishlist-button';
// Products Data
// catergory selected
// Products filtered on catergory
@Component({
  selector: 'app-products-grid',
  imports: [
    ProductCard,
    MatSidenav,
    MatSidenavContainer,
    MatSidenavContent,
    MatNavList,
    MatListItem,
    MatListItemTitle,
    RouterLink,
    TitleCasePipe,
    ToggleWishlistButton,
    MatProgressSpinnerModule,
  ],
  template: `
    <mat-sidenav-container>
      <mat-sidenav mode="side" opened="true">
        <div class="p-6">
          <h2 class="text-lg text-grey-900">Categories</h2>

          <mat-nav-list>
            @for (cat of store.categories(); track cat) {
              <mat-list-item
                [activated]="cat === category()"
                class="my-2"
                [routerLink]="['/products', cat]"
              >
                <span
                  matListItemTitle
                  class="font-medium"
                  [class]="cat === category() ? '!text-white' : null"
                >
                  {{ cat | titlecase }}
                </span>
              </mat-list-item>
            }
          </mat-nav-list>
        </div>
      </mat-sidenav>
      <mat-sidenav-content class="bg-grey-100 p-6 h-full">
        <h1 class="text-2xl font-bold text-gray-900">{{ category() | titlecase }}</h1>
        <p class="text-base text-gray-600 mb-6">
          {{ store.displayProducts().length }} product(s) found
        </p>
        @if (store.loading()) {
          <div class="flex justify-center items-center p-16">
            <mat-spinner diameter="50"></mat-spinner>
          </div>
        } @else if (store.displayProducts().length > 0) {
          <div class="responsive-grid">
            @for (product of store.displayProducts(); track product.id) {
              <app-product-card [product]="product" (addToCartClicked)="store.addToCart(product)">
                <app-toggle-wishlist-button
                  [product]="product"
                  class="!absolute z-10 top-3 right-3 !bg-white shadow-md rounded-md transition-all duration-200 hover:scale-110 hover:shadow-lg"
                  [style.view-transition-name]="'wishlist-button-' + product.id"
                />
              </app-product-card>
            }
          </div>
        } @else {
          <div
            class="flex flex-col items-center justify-center p-12 text-center bg-white rounded-lg shadow-sm"
          >
            <p class="text-lg font-medium text-gray-700">No products found</p>
            <p class="text-sm text-gray-500 mt-1">
              Try searching for something else or picking a different category.
            </p>
          </div>
        }
      </mat-sidenav-content>
    </mat-sidenav-container>
  `,
  styles: ``,
})
export default class ProductsGrid {
  category = input<string>('all'); // Read only input property for category

  addToCart(product: Product) {
    console.log('Adding to cart:', product);
  }

  store = inject(EcommerceStore);

  constructor() {
    this.store.loadCategories();
    this.store.loadProducts();

    effect(() => {
      this.store.setCategory(this.category());
    });
  }
}
