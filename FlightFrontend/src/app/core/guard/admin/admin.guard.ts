import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AuthService } from '../../auth/auth.service';

export const adminGuard: CanActivateFn = (route, state) => {
  const service = inject(AuthService);
  if(service.getRole()==="Admin") return true
  return false;
};
