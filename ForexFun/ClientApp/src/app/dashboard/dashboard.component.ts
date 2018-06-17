import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  ngOnInit(): void {
  }

  public currencies: Currency[];
  public wallets: Wallet[];

  readonly url = "http://localhost:50382/";

  constructor(private http: HttpClient) {
    http.get<Currency[]>(this.url + 'api/dashboard').subscribe(result => {
      this.currencies = result;
      console.log(result);
    }, error => console.error(error));
    http.get<Wallet[]>(this.url + 'api/wallets/' + 1).subscribe(result => {
      this.wallets = result;
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
