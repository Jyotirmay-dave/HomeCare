import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth-service';

@Component({
  selector: 'app-admin-login-component',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './admin-login-component.html',
  styleUrl: './admin-login-component.css',
  standalone: true
})

export class AdminLoginComponent {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private authService = inject(AuthService);

  loginForm: FormGroup;
  showPassword: boolean = false;
  isSubmitting: boolean = false;
  errorMessage: string = "";

  constructor() {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(15), this.passwordValidator]]
    });
  }

  // Custom password validator
  passwordValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    
    if (!value) {
      return null;
    }

    const hasAlphabetic = /[a-zA-Z]/.test(value);
    const hasNumeric = /[0-9]/.test(value);
    const hasSpecialChar = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(value);

    const passwordValid = hasAlphabetic && hasNumeric && hasSpecialChar;

    if (!passwordValid) {
      return { 
        passwordStrength: {
          hasAlphabetic,
          hasNumeric,
          hasSpecialChar
        }
      };
    }

    return null;
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      this.isSubmitting = true;

      const credentials = {
        email: this.loginForm.value.email,
        password: this.loginForm.value.password
      };
      
      // TODO: Implement your API call here
      this.authService.adminLogin(credentials).subscribe({
        next: (response) => {
          console.log('Login successful:', response);
          // Simulate API call
          setTimeout(() => {
            this.isSubmitting = false;
            this.router.navigate(['/admin-dashboard']);
          }, 500);
        },
        error: (error) => {
          console.error('Login failed:', error);
          this.isSubmitting = false;

          // ✅ Handle different error responses
          if (error.status === 401) {
            this.errorMessage = 'Invalid email or password';
          } else if (error.status === 0) {
            this.errorMessage = 'Unable to connect to server. Please try again.';
          } else {
            this.errorMessage = error.error?.message || 'Something went wrong. Please try again.';
          }
        }
      });
    } else {
      this.markFormGroupTouched(this.loginForm);
    }
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }

  onForgotPassword(): void {
    // TODO: Navigate to forgot password page
    console.log('Forgot password clicked');
    // this.router.navigate(['/forgot-password']);
  }

  get email() {
    return this.loginForm.get('email');
  }

  get password() {
    return this.loginForm.get('password');
  }
}