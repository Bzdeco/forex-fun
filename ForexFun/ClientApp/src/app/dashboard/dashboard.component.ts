import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserService } from '../shared/user.service';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/interval';

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
    this.updateState();
    Observable.interval(60000).subscribe((ignore) => {
      this.updateState();
    });
  }
  public currencies: Currency[];
  public wallets: Wallet[];

  readonly url = "http://localhost:50382/";

  constructor(private http: HttpClient, private userService: UserService) { }

  updateState() {
    this.http.get<Currency[]>(this.url + 'api/dashboard').subscribe(result => {
      this.currencies = result;
      console.log(result);
    }, error => console.error(error));

    this.http.get<Wallet[]>(this.url + 'api/wallets/' + 1).subscribe(result => {
      this.wallets = result;
      this.wallets.forEach((wallet, index) => {
        this.http.get<Currency>(this.url + 'api/Currencies/' + wallet.CurrencyId).subscribe(resultCurrency => {
          console.log(wallet.Id);
          wallet.Name = resultCurrency.Name;
        }, error => console.error(error))
      });
    }, error => console.error(error));
  }

  buyCurrency(currencyId, amount) {
    console.log("Currency: " + currencyId);
    console.log("Amount: " + amount);
    var currencyWallet = this.wallets.find(wallet => wallet.CurrencyId === currencyId);
    var usdWallet = this.wallets.find(wallet => wallet.CurrencyId === 1); //TODO
    var exchangeCurrency = this.currencies.find(currency => currency.CurrencyId === currencyId);
    usdWallet.Amount -= Number(amount) * exchangeCurrency.Value;
    console.log("Putting USD wallet: ");
    this.http.put(this.url + "api/wallets/" + usdWallet.Id, usdWallet).subscribe();
    if (currencyWallet === undefined) {
      let newWallet = { UserId: 1, CurrencyId: currencyId, Amount: amount }; //TODO
      console.log("Posting new currency wallet: ");
      console.log(newWallet);
      this.http.post(this.url + "api/wallets", newWallet).subscribe((data: any) => {
        if (data.Succeeded == true) {
          console.log('Success currency');
        }
      });
    }
    else {
      currencyWallet.Amount += Number(amount);
      console.log("Putting currency wallet: ");
      this.http.put(this.url + "api/wallets/" + currencyWallet.Id, currencyWallet).subscribe();
    }
  }

  sellCurrency(currencyId, amount) {
    console.log("Currency: " + currencyId);
    console.log("Amount: " + amount);
    var exchangeCurrency = this.currencies.find(currency => currency.CurrencyId === currencyId);
    var usdWallet = this.wallets.find(wallet => wallet.CurrencyId === 1); //TODO
    usdWallet.Amount += Number(amount) * exchangeCurrency.Value;
    console.log("Putting USD wallet: ");
    this.http.put(this.url + "api/wallets/" + usdWallet.Id, usdWallet).subscribe();
    var currencyWallet = this.wallets.find(wallet => wallet.CurrencyId === currencyId);
    if (Math.abs(Number(amount) - currencyWallet.Amount) < Number.EPSILON) {
      this.http.delete(this.url + "api/wallets/" + currencyWallet.Id).subscribe((data: any) => {
        if (data.Succeeded == true) {
          console.log('Success delete wallet');
        }
      });
    }
    else {
      currencyWallet.Amount -= Number(amount);
      console.log("Putting currency wallet: ");
      this.http.put(this.url + "api/wallets/" + currencyWallet.Id, currencyWallet).subscribe();
    }
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
