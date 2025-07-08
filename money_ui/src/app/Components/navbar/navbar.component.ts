import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { MaterialModule } from '../../material.module';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, MaterialModule, RouterLinkActive, FormsModule],
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
      next: response => { 
        console.log('response: ', response);
        this.username = '';
        this.password = '';
        this.router.navigate(['/']);
      },
      error: error => console.log('error: ', error)
    });
  }

  onLogout() {
    this.accountService.logout();
    this.router.navigate(['/']);
  }
}
