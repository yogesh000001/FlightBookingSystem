import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../../core/auth/auth.service';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [RouterModule, CommonModule,ReactiveFormsModule,FormsModule],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css',
})
export class SignupComponent {
  signupForm: FormGroup;
  loading = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.signupForm = this.fb.group({
      firstName: ['Diljit', [Validators.required, Validators.minLength(2)]],
      lastName: ['Singer', [Validators.required, Validators.minLength(2)]],
      email: ['Diljit@gmail.com', [Validators.required, Validators.email]],
      phone: ['9999999999', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      password: ['Admin@123', [Validators.required, Validators.minLength(6)]],
    });
  }

  submitForm() {
    if (this.signupForm.valid) {
      this.loading = true;
      const payload = {
        firstName: this.signupForm.value.firstName,
        lastName: this.signupForm.value.lastName,
        email: this.signupForm.value.email,
        phoneNo: this.signupForm.value.phone,
        password: this.signupForm.value.password,
        role:"Passenger"
      };

      console.log(payload);
      

      this.authService.login('Users/register', payload).subscribe(
        (res) => {
          this.loading = false;
          if (res?.success) {
            alert('Registration successful! Please login.');
            this.router.navigate(['/login']);
          } else {
            alert('Registration failed. Please try again.');
          }
        },
        (err) => {
          this.loading = false;
          this.router.navigate(['/login']);
          // alert('Server error. Try again later.');
        }
      );
    } else {
      console.log('Invalid form');
    }
  }
}
