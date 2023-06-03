import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent {
  isRightPanelActive: boolean = false;
  isRegisterFormSubmitted: boolean = false;
  isloginFormSubmitted: boolean = false;

  registerForm = this.fb.group({
    username: ['', Validators.required, Validators.minLength(3), Validators.maxLength(20)],
    email: ['', Validators.required, Validators.email],
    password: ['', Validators.required, Validators.minLength(6)]
  });

  loginForm = this.fb.group({
    email: ['', Validators.required, Validators.email],
    password: ['', Validators.required, Validators.minLength(6)]
  });

  constructor(
    private fb: FormBuilder,
    private toastr: ToastrService) { }

  register() {
    this.isRegisterFormSubmitted = true;

    if (!this.registerForm.valid) {
      return;
    }

    this.toastr.error('An error occured! Please try again', 'Error');
  }

  login() {
    this.isloginFormSubmitted = true;

    if (!this.loginForm.valid) {
      return;
    }

    this.toastr.error('An error occured! Please try again', 'Error');
  }

  changePanelActivity() {
    this.isRightPanelActive = !this.isRightPanelActive;
  }

  get registerUsername() {
    return this.registerForm.get('username');
  }

  get registerEmail() {
    return this.registerForm.get('email');
  }

  get registerPassword() {
    return this.registerForm.get('password');
  }

  get loginEmail() {
    return this.loginForm.get('email');
  }

  get loginPassword() {
    return this.loginForm.get('password');
  }
}
