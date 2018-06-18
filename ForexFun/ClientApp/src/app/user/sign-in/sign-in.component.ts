import { Component, OnInit } from '@angular/core';
import { UserService } from '../../shared/user.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent implements OnInit {

  constructor(private userService: UserService, private router: Router) { }

  ngOnInit() {
  }

  OnSubmit(username: string, password: string) {
    this.userService.authenticateUser(username, password).subscribe(
      (data: any) => {
        console.log('Login successful');
        localStorage.setItem('token', data.access_token);
        this.userService.createWallet();
        this.router.navigate(['/dashboard']);
      },
      (err: HttpErrorResponse) => {
        console.log('Login error');
        this.router.navigate(['/'])
      });
  }

  Logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/']);
  }
}
