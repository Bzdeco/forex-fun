import { Component, OnInit } from '@angular/core';
import { User } from '../../shared/user.model';
import { UserService } from '../../shared/user.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {

  user: User;

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.resetForm();
  }

  resetForm(form?: NgForm) {
    if (form != null) {
      form.reset();
    }
    this.user = {
      Username: '',
      Password: ''
    };
  }

  OnSubmit(form: NgForm) {
    console.log('Submission');
    console.log(form.value);
    this.userService.registerUser(form.value)
      .subscribe((data: any) => {
        if (data.Succeeded == true) {
          console.log('Success');
          this.resetForm(form);
        }
      });
  }
}
