import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../../core/auth/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterModule,CommonModule,ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  loginForm: FormGroup;
  loading = false;
  errorMessage = '';
  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['Admin@gmail.com,Aryan@gmail.com', [Validators.email, Validators.required]],
      password: ['Admin@123', [Validators.required, Validators.minLength(4)]],
    });
  }
  submitForm() {
    if (this.loginForm.valid) {
      this.loading = true;
      this.authService.login('Users/login', this.loginForm.value).subscribe(res => {
        if (res?.success) {
          this.authService.setToken(res?.token);
          this.router.navigate(["unknown"])
        } else {
          alert("Invalid Email or Password");  
        }
        
      });
    } else {
      this.loading = false;
      console.log('Invalid from');
    }
  }
}
