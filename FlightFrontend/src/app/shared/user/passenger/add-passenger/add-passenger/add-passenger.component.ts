import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ApiLinkService } from '../../../../../core/api-link/api-link.service';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

  

interface JwtPayload {
  UserId: string;
  
}


@Component({
  selector: 'app-add-passenger',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './add-passenger.component.html',
  styleUrl: './add-passenger.component.css',
})
export class AddPassengerComponent implements OnInit {
  passengerForm!: FormGroup;
  loading = false;
  errorMessage = '';
  userId: string = '';

  constructor(
    private fb: FormBuilder,
    private apiService: ApiLinkService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const token = localStorage.getItem('token');
    if (token) {
      try {
        const decoded = jwtDecode<JwtPayload>(token);
        this.userId = decoded.UserId;
      } catch (error) {
        console.error('Invalid token:', error);
      }
    }

    this.passengerForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      userID: [this.userId, [Validators.required]], // Fill userId from token
      contactDetails: [
        '',
        [Validators.required, Validators.pattern('^[0-9]{10,}$')],
      ],
      code: ['', [Validators.required, Validators.minLength(5)]],
      gender: ['', [Validators.required]],
    });
  }

  submitPassenger() {
    if (this.passengerForm.valid) {
      this.loading = true;
      this.apiService.post('passenger', this.passengerForm.value).subscribe({
        next: (res) => {
          this.passengerForm.reset();
          console.log(res);
          
          this.router.navigate(['/passenger']);
          return 
        },
        error: (err) => {
          console.error('Submit failed:', err);
          this.loading = false;
          this.errorMessage = 'Failed to submit passenger';
        },
        complete: () => {
          this.loading = false;
        },
      });
    } else {
      console.log('Invalid passenger form');
    }
  }
}