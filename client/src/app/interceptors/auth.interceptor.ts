import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
    const authService = inject(AuthService);
    const token = authService.getAccessToken();

    if (!token) {
        return next(req);
    }
    // Angular HTTP requests are immutable. 
    // We clone the request to modify its headers 
    // without altering the original request.
    const authReq = req.clone({
        setHeaders: {
            Authorization: `Bearer ${token}`
        }
    });

    return next(authReq);
};