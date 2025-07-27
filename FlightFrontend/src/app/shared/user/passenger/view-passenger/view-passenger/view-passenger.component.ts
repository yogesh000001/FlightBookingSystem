import { Component, OnInit } from '@angular/core';
import { ApiLinkService } from '../../../../../core/api-link/api-link.service';
import { CommonModule } from '@angular/common';
import { jwtDecode } from 'jwt-decode';


interface JwtPayload {
  UserId: string;
  
}


@Component({
  selector: 'app-view-passenger',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './view-passenger.component.html',
  styleUrl: './view-passenger.component.css',
})
export class ViewPassengerComponent implements OnInit {
  passengers: any[] = [];
  dummy:any[]=[]
  loading = true;
  userId=""

  constructor(private apiService: ApiLinkService) {}

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
    this.apiService.get('passenger').subscribe((res: any) => {
      this.dummy = res;
      this.passengers = this.dummy.filter((u) => {
        console.log(u);
        return u.userID+"" === this.userId+"";
      });
      this.loading = false;
    });
  }
}