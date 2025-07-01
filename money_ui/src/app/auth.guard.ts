import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from './services/account.service';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const router = inject(Router);

  if(accountService.currentUser() != null) {
    return true;
  } else {
    router.navigate(['/']);
    return false;
  }
};
