import {
  HttpInterceptorFn,
  HttpRequest,
  HttpHandlerFn,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { catchError, Observable } from 'rxjs';

export const Intercept: HttpInterceptorFn = (
  req: HttpRequest<any>,
  next: HttpHandlerFn
): Observable<any> => {
  console.log('Intercepting request:', req);

  // Inject AuthService dynamically
  const authService = inject(AuthService);

  // Modify request with Authorization header
  const modifiedReq = req.clone({
    setHeaders: {
      Authorization: `Bearer ${authService.getToken()}`,
    },
  });

  // Handle errors gracefully
  return next(modifiedReq).pipe(
    catchError((error) => {
      console.error('Error in request:', error);
      throw error;
    })
  );
};
