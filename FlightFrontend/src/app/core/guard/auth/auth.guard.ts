import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../auth/auth.service';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  let isValid = inject(AuthService)
  let router=inject(Router);
  
  if (isValid.getRole() === "Admin") {
    router.navigate(["dashboard"])
    return true;

  } 
  else if (isValid.getRole()==="Passenger") {
    router.navigate(["passenger"])
  }
  return false;
};
