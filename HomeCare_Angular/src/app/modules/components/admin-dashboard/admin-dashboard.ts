import { Component, inject } from '@angular/core';
import { AuthService } from '../../../core/services/auth-service';

@Component({
  selector: 'app-admin-dashboard',
  imports: [],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css',
})
export class AdminDashboard {
  private authService = inject(AuthService);

  logout(){
    this.authService.logout();
  }
}
