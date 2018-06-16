import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserService } from '../shared/user.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  Id: string;

  ngOnInit(): void {
    this.userService.getUserId().subscribe(
      (id: any) => { this.Id = id; }
    );
  }
  public currencies: Currency[];

  readonly url = "http://localhost:50382/";

  constructor(private http: HttpClient, private userService: UserService) {
    http.get<Currency[]>(this.url + 'api/dashboard').subscribe(result => {
      this.currencies = result;
      console.log(result);
    }, error => console.error(error));
  }
}

interface Currency {
  name: string;
  value: number;
}
