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
    return this.http.post(this.url + "api/register", postBody);
  }

  authenticateUser(username: string, password: string) {
    var data = "username=" + username + "&password=" + password + "&grant_type=password";
    var requestHeaders = new HttpHeaders({ 'Content-Type': 'application/x-www-urlencoded', 'No-Auth': 'True' });
    return this.http.post(this.url + 'token', data, { headers: requestHeaders });
  }

  getUserId() {
    return this.http.get(this.url + 'api/userid');
  }
}
