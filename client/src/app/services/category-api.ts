import { inject, Injectable, input, signal } from "@angular/core";
import { ProductService } from "./ProductService";
import { EcommerceStore } from "../ecommerce-store";

@Injectable({
    providedIn: 'root',
})
export class CategoryApi{
    // category = input<string>('all'); // Read only input property for category
    categories = signal<string[]>(['All', 'Electronics', 'Home & Kitchen', 'Fitness', 'Accessories', 'Clothing', ]) ;
    productService = inject(ProductService);
    store = inject(EcommerceStore);
    
}