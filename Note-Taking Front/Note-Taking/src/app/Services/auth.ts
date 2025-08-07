import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { API_CONFIG } from '../app.config';
export interface userData{
 
    FullName: string,
    Email: string,
    password: string
 
}
export interface loginData{
   Email: string,
    password: string
}
@Injectable({
  providedIn: 'root'
})

export class Auth {
 private currentUserSubject = new BehaviorSubject<any>(null);
  currentUser$ = this.currentUserSubject.asObservable();
    private tokenExpirationTimer: any;

   constructor(private http: HttpClient,private router: Router) {}

  // login(userData: FormData): Observable<any> {
  //   return this.http.post(`${API_CONFIG.apiUrl}/api/Auth/login`, {
  //   fullName:userData.get('FullName') as string,
  //   email:userData.get('Email') as string,
  //   password:userData.get('password') as string,
  //   });
  // }

  register(userData: FormData): Observable<any> {
    return this.http.post(`${API_CONFIG.apiUrl}/api/Auth/register`, {
    email:userData.get('Email') as string,
        fullName:userData.get('FullName') as string,
    password:userData.get('password') as string,
    });
  
}
login(loginData: FormData): Observable<any> {
  return this.http.post(`${API_CONFIG.apiUrl}/api/Auth/login`, {
    email: loginData.get('Email') as string,
    password: loginData.get('password') as string
  });
}
private redirectToLoginWithMessage(message: string) {
    // Use your router to navigate to login with query params
    this.router.navigate(['/login'], {
      queryParams: { message }
    });
  }

  getToken() {
    return localStorage.getItem('jwtToken') || sessionStorage.getItem('jwtToken');
  }
logout(isExpired = false) {
  localStorage.removeItem('jwtToken');
  sessionStorage.removeItem('jwtToken');
  this.currentUserSubject.next(null);
  if (this.tokenExpirationTimer) {
    clearTimeout(this.tokenExpirationTimer);
  }

  if (isExpired) {
    this.router.navigate(['/login'], {
      queryParams: { message: 'Session expired. Please login again.' }
    });
  } else {
    this.router.navigate(['/login']);
  }
}

}
