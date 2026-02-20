import { Routes } from '@angular/router';
import { AdminLoginComponent } from './modules/components/admin-login-component/admin-login-component';
import { AdminDashboard } from './modules/components/admin-dashboard/admin-dashboard';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
    {path: "", redirectTo: "admin-login", pathMatch: 'full'},
    {path: "admin-login", component: AdminLoginComponent},
    {path: 'admin-dashboard', component: AdminDashboard, canActivate: [authGuard]},
    {path: '**', redirectTo: "admin-login"}
];
