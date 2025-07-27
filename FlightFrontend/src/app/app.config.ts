import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { HttpClientModule, provideHttpClient, withInterceptors } from '@angular/common/http';
import { Intercept } from './core/interceptor/interceptor';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes),provideRouter(routes),provideHttpClient(withInterceptors([Intercept]))]
};
