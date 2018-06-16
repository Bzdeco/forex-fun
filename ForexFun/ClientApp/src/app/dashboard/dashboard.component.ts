import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  ngOnInit(): void {
    throw new Error("Method not implemented.");
  }
  public currencies: Currency[];

  readonly url = "http://localhost:50382/";

  constructor(http: HttpClient) {
    http.get<Currency[]>(this.url + 'api/dashboard').subscribe(result => {
      this.currencies = result;
    }, error => console.error(error));
  }
}

interface Currency {
  name: string;
  value: number;
}
