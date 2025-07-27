import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component} from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiLinkService } from '../../../../core/api-link/api-link.service';

@Component({
  selector: 'app-add-flight',
  standalone: true,
  imports: [CommonModule,FormsModule,ReactiveFormsModule],
  templateUrl: './add-flight.component.html',
  styleUrl: './add-flight.component.css',
})
export class AddFlightComponent {
  flightForm: FormGroup;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private http: ApiLinkService,
    private router: Router
  ) {
    this.flightForm = this.fb.group({
      flightNumber: ['', Validators.required],
      departureTime: ['', Validators.required],
      arrivalTime: ['', Validators.required],
      origin: ['', Validators.required],
      destination: ['', Validators.required],
      status: ['Scheduled', Validators.required],
      totalSeats: [300, [Validators.required, Validators.min(1)]],
    });
  }

  submitForm() {
    if (this.flightForm.valid) {
      this.loading = true;
      const payload = this.flightForm.value;
      console.log(payload);
      
      this.http
        .post('flight', payload)
        .subscribe({
          next: (res) => {
            this.loading = false;
            alert('Flight added successfully!');
            this.router.navigate(['/Dashboard']);
          },
          error: (err) => {
            this.loading = false;
            alert('Error adding flight. Please try again.');
          },
        });
    }
  }
}
