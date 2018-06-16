import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
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
}
