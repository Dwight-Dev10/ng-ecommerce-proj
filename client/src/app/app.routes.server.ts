import { RenderMode, ServerRoute } from '@angular/ssr';
import { CategoryApi } from './services/category-api';
import { inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';

export const serverRoutes: ServerRoute[] = [

  {
    path: 'products/:category',
    renderMode: RenderMode.Prerender,
    getPrerenderParams: async () => {
      const catService = inject(CategoryApi);
      const names = await firstValueFrom(catService.getCategories())
      return names.map((name) => ({ category: name.trim() }));
    }
  },
  {
    path: 'wishlist',
    renderMode: RenderMode.Client,
  },

  {
    path: 'cart',
    renderMode: RenderMode.Client,
  },

  {
    path: 'checkout',
    renderMode: RenderMode.Client,
  },

  {
    path: 'order-success',
    renderMode: RenderMode.Client,
  },

  {
    path: 'product/:productId',
    renderMode: RenderMode.Client,
  },

  {
    path: '**',
    // RenderMode.Prerender will pre-render the page at build time 
    // and serve the static HTML for this route. 
    // This is useful for pages that don't change often and can be cached by CDNs.
    // renderMode: RenderMode.Prerender,
    renderMode: RenderMode.Server,
  },
];
