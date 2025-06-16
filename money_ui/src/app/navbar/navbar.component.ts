import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AccountService } from '../services/account.service';
import { MaterialModule } from '../material.module';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, MaterialModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  username: string = '';
  password: string = '';
  accountService = inject(AccountService);
  router = inject(Router);

  onLogin() {
    let payload = { username: this.username, password: this.password };
    this.accountService.login(payload).subscribe({
      next: response => console.log('response: ', response),
      error: error => console.log('error: ', error)
    });
    this.username = '';
    this.password = '';
    this.router.navigate(['/']);
  }

  onLogout() {
    this.accountService.logout();
    this.router.navigate(['/']);
  }
}
