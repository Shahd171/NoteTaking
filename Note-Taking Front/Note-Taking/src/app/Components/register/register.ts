import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, FormsModule, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { Auth } from '../../Services/auth';


@Component({
  
  selector: 'app-register',
  imports: [ReactiveFormsModule, CommonModule, RouterModule,FormsModule,CommonModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
 
export class Register {
    constructor(private router :Router,private authService: Auth) {
      this.registerform.get('email')?.valueChanges.subscribe(() => {
  const control = this.registerform.get('email');
  if (control?.hasError('This email is already registered.')) {
    control.updateValueAndValidity({ onlySelf: true });
  }
});

    }

  submitted = false;
  showPassword: boolean = false;
showConfirmPassword: boolean = false;

  
  registerform = new FormGroup({
    Name: new FormControl('', [Validators.required, Validators.minLength(2)]),
    email: new FormControl('', [Validators.required, Validators.email]),
 password: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
      this.createPasswordStrengthValidator()
    ]),    confirmPassword: new FormControl('', [Validators.required])
  }, { validators: this.passwordMatchValidator.bind(this) });
passwordMatchValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const formGroup = control as FormGroup;
    const password = formGroup.get('password')?.value;
    const confirmPassword = formGroup.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordMismatch: true };
  }
// Custom password strength validator
  createPasswordStrengthValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      const value = control.value;
      if (!value) return null;
      
      const hasUpperCase = /[A-Z]/.test(value);
      const hasLowerCase = /[a-z]/.test(value);
      const hasNumeric = /[0-9]/.test(value);
      const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(value);
      
      const passwordValid = hasUpperCase && hasLowerCase && hasNumeric && hasSpecialChar;
      
      return !passwordValid ? { 
        passwordStrength: true,
        missingUpperCase: !hasUpperCase,
        missingLowerCase: !hasLowerCase,
        missingNumber: !hasNumeric,
        missingSpecialChar: !hasSpecialChar
      } : null;
    };
  }
  onRegister() {
    
     this.submitted = true;
      this.registerform.markAllAsTouched();

    if (this.registerform.invalid || this.registerform.errors?.['passwordMismatch']) {
      return;
    }

    const data = new FormData();
    data.append('FullName', this.registerform.value.Name!);
    data.append('Email', this.registerform.value.email!);
    data.append('password', this.registerform.value.password!);

    this.authService.register(data).subscribe({
      next: (res) => {
        localStorage.setItem('jwtToken', res.data.token);
        this.router.navigate(['/login']);
      },
      error: (err) => {
  console.error('Registration failed:', err);

  const errorMessage = err.error?.errors?.[0] || 'Unknown error';

  // Show the error under email if it relates to email
  if (errorMessage.toLowerCase().includes('email')) {
    this.registerform.get('email')?.setErrors({ emailTaken: true });
  } else {
    alert('Error: ' + errorMessage);
  }
}

    });
  }
}
