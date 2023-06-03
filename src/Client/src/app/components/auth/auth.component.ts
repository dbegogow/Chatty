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

  registerForm = this.fb.group({
    username: ['', Validators.required, Validators.minLength(3), Validators.maxLength(20)],
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

  changePanelActivity() {
    this.isRightPanelActive = !this.isRightPanelActive;
  }

  get username() {
    return this.registerForm.get('username');
  }

  get email() {
    return this.registerForm.get('email');
  }

  get password() {
    return this.registerForm.get('password');
  }
}
