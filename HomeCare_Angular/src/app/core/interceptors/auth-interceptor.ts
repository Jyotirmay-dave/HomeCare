import { HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../services/auth-service';
import { inject } from '@angular/core';
import { SKIP_AUTH } from './skip-auth.token';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  if(req.context.get(SKIP_AUTH)){
    return next(req);
  }

  const token = authService.getToken();

  // ✅ Attach token to every outgoing request
  if (token) {
    const authReq = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });
    return next(authReq);
  }

  return next(req);
};
