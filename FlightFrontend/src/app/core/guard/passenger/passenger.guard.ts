import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AuthService } from '../../auth/auth.service';

export const passengerGuard: CanActivateFn = (route, state) => {
  const service = inject(AuthService);
  if (service.getRole() === "Passenger") return true;
  return false;
};
