import { Component, inject } from '@angular/core';
import { EcommerceStore } from '../../ecommerce-store';
import { MatInput } from "@angular/material/input";
import { MatIconButton } from '@angular/material/button';
import { MatIcon } from "@angular/material/icon";
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-header-search-bar',
  imports: [MatInput, MatIconButton, MatIcon, FormsModule],
  template: ` 
<form class="search-bar-container" (ngSubmit)="onSearchInput(searchInput.value)">
    <input
      #searchInput
      matInput
      type="text"
      placeholder="Search..."
    />

    <button mat-icon-button type="submit">
      <mat-icon>search</mat-icon>
    </button>
  </form>
  `,
  styles: ``,
})
export class HeaderSearchBar {
  store = inject(EcommerceStore);

  
  onSearchInput(term: string) {
    this.store.searchProducts(term);
  }
  
}
