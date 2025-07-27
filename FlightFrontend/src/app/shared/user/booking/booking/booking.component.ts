import { Component, OnInit } from '@angular/core';
import { ApiLinkService } from '../../../../core/api-link/api-link.service';
import { CommonModule } from '@angular/common';
import { jwtDecode } from 'jwt-decode';


interface JwtPayload {
  UserId: string;

}

@Component({
  selector: 'app-booking',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './booking.component.html',
  styleUrl: './booking.component.css',
})
export class BookingComponent implements OnInit {
  bookings: any[] = [];
  allBookings: any[] = [];
  userId = '';
  // bookings = [
  //   {
  //     bookingID: 1,
  //     flightID: 1,
  //     passengerID: 1,
  //     bookingDate: '2025-07-26T12:51:18.364',
  //     status: 'Pending',
  //     paymentStatus: 'Paid by Customer - pending Confirmation',
  //     seatNumber: 'Not Confirmed',
  //   },
  //   {
  //     bookingID: 2,
  //     flightID: 1,
  //     passengerID: 2,
  //     bookingDate: '2025-07-26T18:30:00',
  //     status: 'Pending',
  //     paymentStatus: 'Paid by Customer - pending Confirmation',
  //     seatNumber: 'Not Confirmed',
  //   },
  // ];

  constructor(private api: ApiLinkService) {}

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

    this.api.get('booking').subscribe((res) => {
      console.log(res);
      this.allBookings = res;
      this.bookings = this.allBookings.filter((b) => {
        return b.userId + '' === this.userId + '';
      });
    });
  }

  cancelTicket(Id: string) {
    this.api
      .post(`Cancellation/Cancel/${Id}`,{})
      .subscribe((res) => {
        console.log(res);
        this.ngOnInit();
      });
  }
}
