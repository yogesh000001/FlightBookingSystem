import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../../core/auth/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent implements OnInit {
  isUser: boolean = false;
  constructor(private auth: AuthService, private route: Router) {}
  ngOnInit(): void {
    this.auth.isLoggedIn$.subscribe((res) => {
      this.auth.isLoggedIn$.subscribe(res => {
        this.isUser=res
      })
    });
  }
  logout() {
    this.auth.logout();
    this.route.navigate([""])
  }

  reRoute() {
    let role = this.auth.getRole();
    if (role === "Admin") this.route.navigate(["/dashboard"])
    else this.route.navigate(["/passenger"])
  }
}
