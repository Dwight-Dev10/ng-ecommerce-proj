import { Component, computed, inject, input } from '@angular/core';
import { EcommerceStore } from '../../ecommerce-store';
import { BackButton } from "../../components/back-button/back-button";
import { ProductInfo } from "./product-info/product-info";
import { ViewReviews } from "./view-reviews/view-reviews";

@Component({
  selector: 'app-view-product-detail',
  imports: [BackButton, ProductInfo, ViewReviews],
  template: ` 
  <div class="mx-auto max-w-[1200px] py-6">
      <app-back-button class="mb-6" [navigateTo]="backRoute()">Continue Shopping</app-back-button>
      @if (store.selectedProduct(); as product) {
        <div class="flex gap-8 mb-8">
          <img [src]="product.imageUrl"
          [style.view-transition-name]="'product-image-' + product.id"
          class="w-[500px] h-[550px] object-cover rounded-lg"
          />
          <div class="flex-1">
            <app-product-info [product]="product"/>
          </div>
        </div>
        <app-view-reviews [product]="product"/>
      }
  </div>
  `,
  styles: ``,
})
export default class ViewProductDetail {

  productId = input.required<string>();
  store = inject(EcommerceStore);

  constructor() {
    this.store.setProductId(this.productId);
  }
  
  backRoute = computed(()=> `/products/${this.store.selectedProduct()?.category || 'all' }`);

}
