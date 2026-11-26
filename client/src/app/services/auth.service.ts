import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

import { environment } from '../../environments/environment';
import { LoginRequest } from '../models/auth/login-request.model';
import { AuthResponse } from '../models/auth/auth-response.model';
import { AuthenticatedUser } from '../models/auth/authenticated-user.model';
import { RegisterRequest } from '../models/auth/register-request.model';

@Injectable({
    providedIn: 'root'
})
export class AuthService {

    private http = inject(HttpClient);

    private apiUrl = `${environment.apiUrl}/auth`;

    private accessToken = signal<string | null>(null);

    register(request: RegisterRequest): Observable<void> {
        return this.http.post<void>(`${this.apiUrl}/register`, request);
    }

    login(request: LoginRequest): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(
            `${this.apiUrl}/login`,
            request
        ).pipe(
            tap(response => this.accessToken.set(response.accessToken))
        );
    }

    getAccessToken(): string | null {
        return this.accessToken();
    }

    getMe(): Observable<AuthenticatedUser> {
        return this.http.get<AuthenticatedUser>(`${this.apiUrl}/me`);
    }

    clearToken(): void {
        this.accessToken.set(null);
    }
}