import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from './user.model';

@Injectable()
export class UserService {

  readonly url = "http://localhost:50382/";

  constructor(private http: HttpClient) { }

  registerUser(user: User) {
    const postBody: User = {
      Username: user.Username,
      Password: user.Password
    };
    var requestHeaders = new HttpHeaders({ 'No-Auth': 'True' });
    return this.http.post(this.url + "api/register", postBody, { headers: requestHeaders });
  }

  authenticateUser(username: string, password: string) {
    var data = "username=" + username + "&password=" + password + "&grant_type=password";
    var requestHeaders = new HttpHeaders({ 'Content-Type': 'application/x-www-urlencoded', 'No-Auth': 'True' });
    return this.http.post(this.url + 'token', data, { headers: requestHeaders });
  }

  getUserId() {
    return this.http.get(this.url + 'api/userid');
  }

  createWallet() {
    this.getUserId().subscribe(
      (id: string) => {
        this.http.get(this.url + 'api/wallets/' + id).subscribe(
          (data: any) => {
            const wallet = {
              UserId: id,
              CurrencyId: 1,
              Amount: 10000
            };
            console.log(data);
            if (data.length === 0) {
              console.log("Creating wallet");
              this.http.post(this.url + 'api/wallets', wallet).subscribe((data) => console.log("Succeececssc"));
            }
          }
        );
      });
  }
}
