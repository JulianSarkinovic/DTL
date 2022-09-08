import { NgModule } from '@angular/core';
import { AuthGuardService } from './_shared/services/auth-guard.service';
import { Routes, RouterModule } from '@angular/router';
import { RankingComponent } from './_components/ranking/ranking.component';
import { AddMatchComponent } from './_components/add-match/add-match.component';
import { ProfileComponent } from './_components/account/profile/profile.component';
import { E404Component } from './_components/e404/e404.component';
import { RegisterComponent } from './_components/account/register/register.component';
import { LoginComponent } from './_components/account/login/login.component';
import { PasswordForgotComponent } from './_components/account/password-forgot/password-forgot.component';
import { TestComponent } from './_coding/test/test.component';
import { EmailConfirmationComponent } from './_components/account/register/email-confirmation/email-confirmation.component';
import { MatchConfirmationComponent } from './_components/add-match/match-confirmation/match-confirmation.component';
import { MatchConfirmedComponent } from './_components/add-match/match-confirmed/match-confirmed.component';
import { EmailConfirmedComponent } from './_components/account/register/email-confirmed/email-confirmed.component';
import { PasswordResetComponent } from '@components/account/password-reset/password-reset.component';
import { PasswordChangeComponent } from '@components/account/password-change/password-change.component';

const routes: Routes = [
  { path: '', component: RankingComponent },
  { path: 'test', component: TestComponent },
  { path: '404', component: E404Component },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'ranking', component: RankingComponent },
  { path: 'password-forgot', component: PasswordForgotComponent },
  { path: 'password-reset', component: PasswordResetComponent },
  { path: 'match-confirmation', component: MatchConfirmationComponent },
  { path: 'match-confirmed', component: MatchConfirmedComponent },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthGuardService],
  },
  {
    path: 'email-confirmation',
    component: EmailConfirmationComponent,
    canActivate: [AuthGuardService],
  },
  {
    path: 'email-confirmed',
    component: EmailConfirmedComponent,
    canActivate: [AuthGuardService],
  },
  {
    path: 'password-change',
    component: PasswordChangeComponent,
    canActivate: [AuthGuardService],
  },
  {
    path: 'add-match',
    component: AddMatchComponent,
    canActivate: [AuthGuardService],
  },
  { path: '**', redirectTo: '404', pathMatch: 'full' },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      relativeLinkResolution: 'corrected',
      scrollPositionRestoration: 'enabled',
    }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
