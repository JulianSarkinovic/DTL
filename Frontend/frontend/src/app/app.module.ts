import { environment } from 'src/environments/environment';
import { TokenService } from '@services/auth-token.service';

import { AppRoutingModule } from './app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NotifierModule } from 'angular-notifier';
import { HttpClientModule } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';

import { AppComponent } from '@components/root/app.component';
import { HeaderComponent } from '@components/navigation/header/header.component';
import { FooterComponent } from '@components/navigation/footer/footer.component';
import { RankingComponent } from '@components/ranking/ranking.component';
import { ProfileComponent } from '@components/account/profile/profile.component';
import { E404Component } from '@components/e404/e404.component';
import { LoginComponent } from '@components/account/login/login.component';
import { RegisterComponent } from '@components/account/register/register.component';
import { PasswordForgotComponent } from '@components/account/password-forgot/password-forgot.component';
import { AddMatchComponent } from '@components/add-match/add-match.component';
import { AddByNameComponent } from '@components/add-match/match-opponent/add-by-name/add-by-name.component';
import { MatchScoreComponent } from '@components/add-match/match-score/match-score.component';
import { MatchClubComponent } from '@components/add-match/match-club/match-club.component';

import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatOptionModule, MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';

import { NgxSliderModule } from '@angular-slider/ngx-slider';

import { AuthInterceptorProviders } from '@shared/interceptors/auth.interceptor';
import { MatchDurationComponent } from '@components/add-match/match-duration/match-duration.component';
import { MatchRankedComponent } from '@components/add-match/match-ranked/match-ranked.component';
import { FilterUserPipe } from '@shared/pipes/filter-user.pipe';
import { AddByEmailComponent } from '@components/add-match/match-opponent/add-by-email/add-by-email.component';
import { TestComponent } from './_coding/test/test.component';
import { NotifierWrapService } from '@services/notifier-wrap.service';
import { EmailConfirmationComponent } from '@components/account/register/email-confirmation/email-confirmation.component';
import { EmailConfirmedComponent } from '@components/account/register/email-confirmed/email-confirmed.component';
import { MatchScoreViewComponent } from '@components/add-match/match-score-view/match-score-view.component';
import { MatchConfirmationComponent } from '@components/add-match/match-confirmation/match-confirmation.component';
import { MatchConfirmedComponent } from '@components/add-match/match-confirmed/match-confirmed.component';
import { ConfirmPasswordValidatorDirective } from '@shared/directives/confirm-password-validator.directive';
import { MatchSurfaceComponent } from '@components/add-match/match-surface/match-surface.component';
import { MatchConfirmationDialogComponent } from './_components/add-match/match-confirmation/match-confirmation-dialog/match-confirmation-dialog.component';
import { PasswordResetComponent } from './_components/account/password-reset/password-reset.component';
import { PasswordChangeComponent } from './_components/account/password-change/password-change.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    RankingComponent,
    ProfileComponent,
    E404Component,
    LoginComponent,
    RegisterComponent,
    AddMatchComponent,
    PasswordForgotComponent,
    AddByNameComponent,
    MatchScoreComponent,
    MatchClubComponent,
    MatchDurationComponent,
    MatchRankedComponent,
    FilterUserPipe,
    AddByEmailComponent,
    TestComponent,
    EmailConfirmationComponent,
    EmailConfirmedComponent,
    MatchScoreViewComponent,
    MatchConfirmationComponent,
    MatchConfirmedComponent,
    ConfirmPasswordValidatorDirective,
    MatchSurfaceComponent,
    MatchConfirmationDialogComponent,
    PasswordResetComponent,
    PasswordChangeComponent,
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    FlexLayoutModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,

    MatAutocompleteModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatDatepickerModule,
    MatDialogModule,
    MatDividerModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatOptionModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    MatSidenavModule,
    MatSlideToggleModule,
    MatStepperModule,
    MatTableModule,
    MatToolbarModule,

    NgxSliderModule,

    JwtModule.forRoot({
      config: {
        tokenGetter: () => TokenService.GetTokenString(),
        allowedDomains: [environment.urlAPIBase],
        disallowedRoutes: [],
      },
    }),

    NotifierModule.withConfig({
      position: {
        horizontal: {
          position: 'right',
          distance: NotifierWrapService.GetNotifierPosition(),
        },
        vertical: {
          position: 'bottom',
          distance: 80,
        },
      },
      theme: 'material',
      behaviour: {
        autoHide: 6000,
        onClick: 'hide',
        onMouseover: 'pauseAutoHide',
        showDismissButton: false,
      },
      animations: {
        show: {
          preset: 'fade',
          easing: 'ease-in-out',
        },
      },
    }),
  ],
  providers: [AuthInterceptorProviders],
  bootstrap: [AppComponent],
})
export class AppModule {}
