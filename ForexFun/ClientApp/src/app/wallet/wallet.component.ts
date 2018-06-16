import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

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
    http.get<Wallet[]>(this.url + 'api/wallets').subscribe(result => {
      this.wallets = result;
      console.log(result);
    }, error => console.error(error));
  }

  sellCurrency(currencyId, amount) {
    console.log("Currency: " + currencyId);
    console.log("Amount: " + amount);
  }
}

interface Wallet {
  userId: number;
  currencyId: number;
  amount: number;
}
