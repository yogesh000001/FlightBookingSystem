import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ApiLinkService } from '../../../../core/api-link/api-link.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-passenger',
  standalone: true,
  imports: [CommonModule, FormsModule,RouterModule],
  templateUrl: './passenger.component.html',
  styleUrl: './passenger.component.css',
})
export class PassengerComponent implements OnInit {
  constructor(private api: ApiLinkService) {}
  allFlights: any[] = [];
  upcomingFlights: any[] = [];
  ngOnInit(): void {
    this.api.get('flight').subscribe((res) => {
      this.allFlights = res;
      this.upcomingFlights = this.allFlights.filter((e) => {
        return new Date(e.departureTime).getTime() >= Date.now();
      });

      console.log(res);
    });
  }
  searchId: string = '';
  searchedFlight: any = null;

  searchFlight() {
    if (this.searchId.trim()) {
      this.upcomingFlights = this.allFlights.filter((e) => {
        return new Date(e.departureTime).getTime() >= Date.now();
      });

      this.upcomingFlights = this.upcomingFlights.filter(f => {
        console.log(f.flightID+this.searchId+"");
        
        return f?.flightID+""=== this.searchId+"".trim()
      })
    } else {
      
      this.upcomingFlights = this.allFlights.filter((e) => {
        return new Date(e.departureTime).getTime() >= Date.now();
      });
    }
  }
}
