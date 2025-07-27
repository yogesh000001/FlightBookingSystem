import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, catchError, Observable, tap, throwError } from 'rxjs';
import { jwtDecode, JwtDecodeOptions, JwtPayload } from 'jwt-decode';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  router = inject(Router);

  baseUrl = 'http://localhost:5210';

  private loginStatus = new BehaviorSubject<boolean>(!!this.getToken());
  isLoggedIn$ = this.loginStatus.asObservable();
  constructor(private http: HttpClient) {}

  login(endpoint: string, data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/' + endpoint, data).pipe(
      tap((res: any) => {
        if (res.success) {
          this.loginStatus.next(true); // Set loginStatus to true only if login is successful
        } else {
          this.loginStatus.next(false); // Keep it false if login fails
        }
      }),
      catchError((error) => {
        this.loginStatus.next(false); // Ensure status remains false on error
        return throwError(error);
      })
    );
  }

  setToken(token: string) {
    
    localStorage.setItem('token', token);
    console.log('token is set');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  logout() {
    if (this.getToken() === null) {
      console.log('cant logout because u were never loggedin');
      return;
    }
    localStorage.removeItem('token');
    this.loginStatus.next(false);
    console.log('token is removed');
    this.router.navigate(['/']);
  }

  getRole() {
    let token = this.getToken();
    // console.log(token);

    if (token === null) return null;
    try {
      const decodeJWT: any = jwtDecode(token);
      
      const role =
        decodeJWT[
          'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
        ];
      console.log(role);

      return role;
    } catch (error) {
      console.log('Invalid token', error);
      this.logout();
      return null;
    }
  }

  isLoggedIn() {
    return this.getToken() === null ? false : true;
  }
}
