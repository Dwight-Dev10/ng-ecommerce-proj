import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Product } from '../models/product';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { privateDecrypt } from 'node:crypto';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private http = inject(HttpClient);

  private apiUrl = `${environment.apiUrl}/products`;

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(
      this.apiUrl
    );
  }
  
  getProductsByName(name: string): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}?name=${name}`);
  }

  getCategories(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/categories`);
  }

  getSearchForProduct(searchTerm: string): Observable<Product[]>{
    return this.http.get<Product[]>(`${this.apiUrl}/search=${searchTerm}`);
  }

  createProduct(product: Product): Observable<Product>{
    return this.http.post<Product>(this.apiUrl, product);
  }

  updateProduct(product: Product): Observable<Product>{
    return this.http.put<Product>(`${this.apiUrl}/${product.id}`, product);
  }

  deleteProduct(productId: number): Observable<void>{
    return this.http.delete<void>(`${this.apiUrl}/${productId}`)
  }


}