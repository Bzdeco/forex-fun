import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../shared/user.model';

type UserRecord = [string, number];

@Component({
  selector: 'app-ranking',
  templateUrl: './ranking.component.html',
  styleUrls: ['./ranking.component.css']
})
export class RankingComponent implements OnInit {

  readonly url = "http://localhost:50382/";

  public currencies: Currency[];
  public wallets: Wallet[];
  public userFortunes: UserFortune[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.http.get<Currency[]>(this.url + 'api/dashboard').subscribe(result => {
      this.currencies = result;
    }, error => console.error(error));
    this.http.get<Wallet[]>(this.url + 'api/wallets/').subscribe(result => {
      this.wallets = result;
      this.wallets.forEach((wallet, index) => {
        this.http.get<Currency>(this.url + 'api/Currencies/' + wallet.CurrencyId).subscribe(resultCurrency => {
          wallet.Name = resultCurrency.Name;
          this.http.get<NamedUser>(this.url + 'api/users/' + wallet.UserId).subscribe(resultUser => {
            var exchangeRate = this.currencies.find(currency => currency.CurrencyId === wallet.CurrencyId).Value;
            var usdValue = wallet.Amount * exchangeRate;
            var currentFortune = this.userFortunes.find(userFortune => userFortune.Name === resultUser.Username);
            if (currentFortune === undefined) {
              this.userFortunes.push({ Name: resultUser.Username, Fortune: usdValue });
            }
            else {
              this.userFortunes.find(userFortune => userFortune.Name === resultUser.Username).Fortune = usdValue + currentFortune.Fortune;
            }
              this.userFortunes.sort((a, b): number => {
                return b.Fortune - a.Fortune;
            });
          }, error => console.error(error))
        });
      }, error => console.error(error));
      
    });
  }

}

interface Wallet {
  Id: number;
  UserId: number;
  CurrencyId: number;
  Amount: number;
  Name: string;
}

interface Currency {
  CurrencyId: number;
  Name: string;
  Value: number;
}

interface UserFortune {
  Name: string;
  Fortune: number;
}

interface NamedUser {
  UserId: string;
  Username: string;
}
