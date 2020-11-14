import { Component, OnInit } from '@angular/core';
import { AuthService } from '../Services/Auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  constructor(private authService: AuthService){ }
  isLoggedIn: boolean = false;
  isAdmin: boolean = false;
  isExpanded = false;
  
  ngOnInit(){
    this.isAdmin = this.authService.isAdmin();
    this.isLoggedIn = this.authService.isLoggedIn();

    this.authService.statusLogin.subscribe(
      (status) => {
        this.isLoggedIn = status;
        this.isAdmin = this.authService.isAdmin();
      }
    );
  }
  logout()
  {
    this.authService.LogOut();
  }
  collapse() {
    this.isExpanded = false;
  }
  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
