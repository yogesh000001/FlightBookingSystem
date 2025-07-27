import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../../core/auth/auth.service';
import { ApiLinkService } from '../../../../core/api-link/api-link.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule,RouterModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent implements OnInit {
  bookings: any[] = [];
  flights: any[] = [];
  constructor(private http:ApiLinkService){}
  ngOnInit(): void {
    // Replace with real API call
    this.http.get('Booking').subscribe((res) => {
      if (res.length > 0) {
        console.log(res);
        
        this.bookings = res
        return;
      }
      this.bookings=[]
    });
    // this.bookings = [
    //   {
    //     bookingID: 1,
    //     flightID: 101,
    //     passengerID: 201,
    //     bookingDate: new Date(),
    //     status: 'Confirmed',
    //     paymentStatus: 'Paid',
    //     seatNumber: '12A',
    //   },
    //   {
    //     bookingID: 2,
    //     flightID: 102,
    //     passengerID: 202,
    //     bookingDate: new Date(),
    //     status: 'Pending',
    //     paymentStatus: 'Unpaid',
    //     seatNumber: '7C',
    //   },
    // ];

    this.http.get("flight").subscribe(res => {
      console.log(res);
      this.flights=res
      
    })

    // this.flights = [
    //   {
    //     flightID: 101,
    //     flightNumber: 'AI202',
    //     departureTime: new Date(),
    //     arrivalTime: new Date(),
    //     origin: 'Delhi',
    //     destination: 'Mumbai',
    //     status: 'On Time',
    //     totalSeats: 180,
    //     bookings: [1, 2],
    //   },
    //   {
    //     flightID: 102,
    //     flightNumber: 'SG301',
    //     departureTime: new Date(),
    //     arrivalTime: new Date(),
    //     origin: 'Mumbai',
    //     destination: 'Chennai',
    //     status: 'Delayed',
    //     totalSeats: 160,
    //     bookings: [],
    //   },
    // ];
  }

  cancelRequest(Id: string) {
    this.http.put(`Cancellation/ApproveCancellation/${Id}`, {}).subscribe({
      next: (res) => {
        this.ngOnInit();
        console.log(res);
      },
      error: (err) => {
        console.error('Error approving cancellation:', err);
      },
      complete: () => {
        this.ngOnInit();
        console.log('Cancellation approval request completed.');
      },
    });
  }
  
}
