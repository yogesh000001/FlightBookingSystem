import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiLinkService } from '../../../../../core/api-link/api-link.service';

@Component({
  selector: 'app-book-flight',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule,FormsModule],
  templateUrl: './book-flight.component.html',
  styleUrl: './book-flight.component.css',
})
export class BookFlightComponent {
  bookForm: FormGroup;
  loading = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private bookingService: ApiLinkService,
    private router: Router
  ) {
    this.bookForm = this.fb.group({
      flightID: [0, [Validators.required]],
      passengerID: [0, [Validators.required]],
      bookingDate: [new Date().toISOString(), [Validators.required]],
    });
  }

  submitBooking() {
    if (this.bookForm.valid) {
      this.loading = true;
      this.bookingService.post("booking", this.bookForm.value).subscribe((res) => {
        this.bookForm.reset();
        this.loading = false;
        this.router.navigate(['passenger-booking']);
        
      });
      
      // console.log(this.bookForm.value);
    } else {
      this.loading=false
      console.log('Invalid booking form');
    }
  }
}