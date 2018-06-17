import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
  selector: 'app-wallet',
  templateUrl: './wallet.component.html',
  styleUrls: ['./wallet.component.css']
})
export class WalletComponent implements OnInit {

  ngOnInit(): void {
  }

  public wallets: Wallet[];

  readonly url = "http://localhost:50382/";

  constructor(http: HttpClient) {
    http.get<Wallet[]>(this.url + 'api/wallets/' + 1).subscribe(result => {
      this.wallets = result;
      this.wallets.forEach((wallet, index) => {
        http.get<Currency>(this.url + 'api/Currencies/' + wallet.CurrencyId).subscribe(resultCurrency => {
          console.log(wallet.Id);
          wallet.Name = resultCurrency.Name;
        }, error => console.error(error))
      });
    }, error => console.error(error));
  }

  sellCurrency(currencyId, amount) {
    console.log("Currency: " + currencyId);
    console.log("Amount: " + amount);
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
  Id: number;
  Name: string;
}
