import { inject, Injectable } from "@angular/core";
import { ProductService } from "./ProductService";

@Injectable({
    providedIn: 'root',
})
export class CategoryApi{
    private categories = ['All', 'Electronics', 'Home & Kitchen', 'Fitness', 'Accessories', 'Clothing', ];
    productService = inject(ProductService);
    getCategories(){
        // return this.categories;
        return this.productService.getCategories();
    }
}