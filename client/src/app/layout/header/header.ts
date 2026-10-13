import { Component } from '@angular/core';
import { MatToolbar } from '@angular/material/toolbar';
import { HeaderActions } from "../header-actions/header-actions";
import { HeaderSearchBar } from "../header-search-bar/header-search-bar";
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-header',
  imports: [MatToolbar, HeaderActions, HeaderSearchBar,RouterLink],
  template: ` 
  <mat-toolbar class="w-full elevated py-2">
    <div class="max-w-[1200px] mx-auto w-full flex items-center justify-between">
      <a mat-button routerLink="/" class="text-xl font-bold">Modern E-commerce</a>
      <app-header-search-bar/>
      <app-header-actions/>
    </div>
  </mat-toolbar>
  `,
  styles: ``,
})
export class Header {}
