import { HttpClient, HttpContext, HttpContextToken } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { Observable, tap } from 'rxjs';
import { SKIP_AUTH } from '../interceptors/skip-auth.token';

export interface AdminLoginRequest {
  email: string;
  password: string;
}

export interface AdminUserResponse {
  id: number;
  userName: string;
  email: string;
  isActive: boolean;
}

export interface AdminLoginResponse {
  response: AdminUserResponse;
  token: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);

  private apiurl = "http://localhost:5098/api";
  private tokenKey = "";
  private refreshTokenKey = "";

  adminLogin(credentials: AdminLoginRequest): Observable<AdminLoginResponse> {
    return this.http.post<AdminLoginResponse>(
      `${this.apiurl}/Admin/Login`, 
      credentials, 
      { 
        context: new HttpContext().set(SKIP_AUTH, true)
      }).pipe(
      tap((response) => {
        if (response.token) {
          this.saveToken(response.token);
          localStorage.setItem("userInfo", JSON.stringify(response.response));
        }
      })
    );
  }

  // ✅ Save Token
  saveToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  // ✅ Save Refresh Token
  saveRefreshToken(refreshToken: string): void {
    localStorage.setItem(this.refreshTokenKey, refreshToken);
  }

  // ✅ Get Token
  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  // ✅ Check if logged in
  isLoggedIn(): boolean {
    const token = this.getToken();
    if(!token)  return false;

    try {
      const decoded: any = jwtDecode(token);
      console.log(decoded);
      const currTime = Math.floor(Date.now() / 1000);
      if(decoded.exp < currTime){
        this.logout();
        return false;
      }
      return true;
    } catch (error) {
      this.logout();
      return false;
    }
  }

  // ✅ Logout - Clear token
  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.refreshTokenKey);
    this.router.navigate(['/admin-login']);
  }
}
