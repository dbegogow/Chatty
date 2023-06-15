import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { RegisterModel } from 'src/app/models/request/register.model';
import { LoginModel } from 'src/app/models/request/login.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent {
  isRightPanelActive: boolean = false;
  isRegisterFormSubmitted: boolean = false;
  isloginFormSubmitted: boolean = false;
  isFormLoading: boolean = false;

  registerForm = this.fb.group({
    username: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private toastr: ToastrService,
    private authService: AuthService) { }

  register() {
    this.isRegisterFormSubmitted = true;
    this.isFormLoading = true;

    if (!this.registerForm.valid) {
      this.isFormLoading = false;
      return;
    }

    const user: RegisterModel = {
      username: this.registerUsername?.value,
      email: this.registerEmail?.value,
      password: this.registerPassword?.value
    };

    this.authService.register(user)
      .subscribe({
        next: res => {
          this.toastr.success('Registered successfully');

          this.authService.saveToken(res.token);

          this.router.navigate(['chats']);
        },
        error: (errorRes) => {
          errorRes.error.forEach((err: any, index: number) => {
            setTimeout(() => {
              this.toastr.error(err.description);
            }, 500 * index);
          });
        },
      }).add(() => {
        this.isRegisterFormSubmitted = false;
        this.isFormLoading = false;
      });
  }

  login() {
    this.isloginFormSubmitted = true;
    this.isFormLoading = true;

    if (!this.loginForm.valid) {
      this.isFormLoading = false;
      return;
    }

    this.isloginFormSubmitted = false;

    const user: LoginModel = {
      email: this.loginEmail?.value,
      password: this.loginPassword?.value
    };

    this.authService.login(user)
      .subscribe({
        next: res => {
          this.toastr.success('Login successfully');

          this.authService.saveToken(res.token);

          this.router.navigate(['chats']);
        },
        error: () => {
          this.isloginFormSubmitted = true;
        },
      }).add(() => {
        this.isFormLoading = false;
      });
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
