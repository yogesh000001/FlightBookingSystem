import { Routes } from '@angular/router';
import { HomeComponent } from './shared/pages/home/home/home.component';
import { LoginComponent } from './shared/pages/login/login/login.component';
import { SignupComponent } from './shared/pages/signup/signup/signup.component';
import { DashboardComponent } from './shared/admin/dashboard/dashboard/dashboard.component';
import { authGuard } from './core/guard/auth/auth.guard';
import { PassengerComponent } from './shared/user/passenger/passenger/passenger.component';
import { adminGuard } from './core/guard/admin/admin.guard';
import { passengerGuard } from './core/guard/passenger/passenger.guard';
import { ShimmerComponent } from './shared/pages/shimmer/shimmer/shimmer.component';
import { BookFlightComponent } from './shared/user/passenger/book-flight/book-flight/book-flight.component';
import { BookingComponent } from './shared/user/booking/booking/booking.component';
import { AddPassengerComponent } from './shared/user/passenger/add-passenger/add-passenger/add-passenger.component';
import { ViewPassengerComponent } from './shared/user/passenger/view-passenger/view-passenger/view-passenger.component';
import { AddFlightComponent } from './shared/admin/add-flight/add-flight/add-flight.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    title: 'Flight Booking Site',
  },
  {
    path: 'login',
    component: LoginComponent,
    title: 'Login-Form',
  },
  {
    path: 'signup',
    component: SignupComponent,
    title: 'Singup-Form',
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [adminGuard],
    title: 'Admin-Dashboard',
  },
  {
    path: 'passenger',
    component: PassengerComponent,
    canActivate: [passengerGuard],
    title: 'Passenger-Dashboard',
  },
  {
    path: 'unknown',
    component: ShimmerComponent,
    canActivate: [authGuard],
  },
  {
    path: 'book-a-flight',
    component: BookFlightComponent,
    canActivate: [passengerGuard],
    title: 'Flight-Book',
  },
  {
    path: 'passenger-booking',
    component: BookingComponent,
    canActivate: [passengerGuard],
    title: 'Passengers List',
  },
  {
    path: 'Add-a-passenger',
    component: AddPassengerComponent,
    canActivate: [passengerGuard],
    title: 'Passenger-Add',
  },
  {
    path: "view-passenger",
    component:ViewPassengerComponent,canActivate:[passengerGuard],title:"View-Passenger"
    
  },
  {
    path: "add-flight",
    component: AddFlightComponent,
    canActivate: [adminGuard],
    title:"Add-Flight"
  }
];
