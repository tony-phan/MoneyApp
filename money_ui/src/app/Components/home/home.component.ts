import { Component, inject, OnInit } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { RegisterComponent } from "../register/register.component";
import { MaterialModule } from '../../material.module';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent, MaterialModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  authService = inject(AccountService);
  registerMode = false;

  ngOnInit(): void {
    this.setCurrentUser()
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if(!userString) return;
    const user = JSON.parse(userString);
    this.authService.currentUser.set(user);
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }

  isLoggedIn(): boolean {
    return !!this.authService.currentUser(); // assuming currentUser is a signal
  }

}
