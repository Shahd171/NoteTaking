import { Component } from '@angular/core';
import bootstrap from '../../../main.server';
import { Router, RouterModule } from '@angular/router';
import { Auth } from '../../Services/auth';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, CommonModule, RouterModule,FormsModule,CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
      constructor(private router :Router,private authService: Auth) {}
loginform=new FormGroup({
    email:new FormControl('',[Validators.required, Validators.email]),
      password:new FormControl('',[Validators.required, Validators.minLength(6)]),
});
  showPassword: boolean = false;


 onLogin() {
  //  this.loginform.markAllAsTouched();

  // if (this.loginform.invalid) {
  //   return;
  // }
  console.log("ahhhhhhh");

    const data = new FormData();
    data.append('Email', this.loginform.value.email!);
    data.append('password', this.loginform.value.password!);

    this.authService.login(data).subscribe({
      next: (res) => {
        console.log('Login success:', res);
        localStorage.setItem('jwtToken', res.data.token); // Adjust if response shape differs
        this.router.navigate(['/Home']); // or your homepage
      },
      error: (err) => {
        console.error('Login failed:', err);
        alert('Error: ' + (err.error?.errors?.join(', ') || 'Invalid email or password'));
      }
    });
  }
}
