import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { UserService } from './shared/user.service';
import { UserComponent } from './user/user.component';
import { SignUpComponent } from './user/sign-up/sign-up.component';
import { SignInComponent } from './user/sign-in/sign-in.component';
import { AuthenticationGuard } from './authentication/authentication.guard';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AuthenticationInterceptor } from './authentication/authentication-interceptor';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    SignUpComponent,
    UserComponent,
    SignInComponent,
    DashboardComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: UserComponent, pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent, canActivate: [AuthenticationGuard] }
    ])
  ],
  providers: [
    UserService,
    AuthenticationGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthenticationInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
