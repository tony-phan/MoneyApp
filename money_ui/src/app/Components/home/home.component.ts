import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { AccountService } from '../../services/account.service';
import { MaterialModule } from '../../material.module';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent, MaterialModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  private accountService = inject(AccountService);
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
    this.accountService.currentUser.set(user);
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }

  isLoggedIn(): boolean {
    return this.accountService.currentUser() != null; // call it like a function if it's a signal
  }

}
