import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { jwtInterceptor } from './Interceptors/jwt-interceptor';
export const API_CONFIG = {
  // apiUrl: 'https://localhost:7184/',
  apiUrl: 'https://localhost:7008',
};
export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes), provideClientHydration(withEventReplay()),
    provideHttpClient(withInterceptorsFromDi()),
     {
      provide: HTTP_INTERCEPTORS,
      useClass: jwtInterceptor,
      multi: true,
    },
  ]
  
};
