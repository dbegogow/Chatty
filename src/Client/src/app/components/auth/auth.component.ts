import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent {
  isRightPanelActive: boolean = false;

  registerForm = this.fb.group({
    username: ['', Validators.required, Validators.minLength(3), Validators.maxLength(20)],
    email: ['', Validators.required, Validators.email],
    password: ['', Validators.required, Validators.minLength(6)]
  });

  constructor(private fb: FormBuilder) { }

  register() {
    console.log('register');
  }

  changePanelActivity() {
    this.isRightPanelActive = !this.isRightPanelActive;
  }
}
