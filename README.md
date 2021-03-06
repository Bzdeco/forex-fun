# ForexFun
Forex-like trading system to try one's hand at trading on cryptocurrency markets

# Usage
To run ForexFun requires:
- PostgreSQL running locally on _localhost:5432_ with user _postgres_ and password _postgres_,
- DatabaseService running (operates on PostgreSQL DB and is queried by Angular app)

ForexFun is an Angular 5 Single Page Application, can be run from Visual Studio or by running `ng serve` in `ForexFun/ClientApp`.

In DatabaseService there is an example `RecordsPublisher` publishing database wallets count to Kafka. `RecordsPublisher` uses a task which publishes to Kafka with a configurable frequency.

# Credits
Web API Token Based Authentication and Registration with Angular 5 done with tutorials:
- [Tutorial Part 1 - Web API Registration](http://www.dotnetmob.com/angular-5-tutorial/angular-5-user-registration-web-api/)
- [Tutorial Part 2 - Web API Token Based Authentication Login and Logout](http://www.dotnetmob.com/angular-5-tutorial/angular-5-login-and-logout-with-web-api-using-token-based-authentication/)

PostgreSQL port for ASP.NET Identity:
- [ASP.NET Identity provider using EntityFramework for PostgreSQL](https://github.com/vincechan/PostgreSQL.AspNet.Identity.EntityFramework)
