import { ChangeDetectionStrategy, Component, computed, inject, OnInit, Signal } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import {MatToolbarModule} from '@angular/material/toolbar';
import { SpinnerComponent } from './components/common/spinner/spinner.component';
import { MatIcon } from '@angular/material/icon';
import { AccountService } from 'src/app/services/account.service';
import { MatIconButton } from '@angular/material/button';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    MatToolbarModule,
    SpinnerComponent,
    MatIconButton,
    MatIcon
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AppComponent implements OnInit {

  private readonly _router: Router = inject(Router);
  private readonly _accountService: AccountService = inject(AccountService);

  public title = 'client';

  public readonly isLoggedIn: Signal<boolean> = computed(
    () => !!this._accountService.currentUser()
  )

  public readonly userName: Signal<string> = computed(
    () => this._accountService.currentUser()?.userName ?? ""
  )

  ngOnInit(): void {
    if (!this._accountService.currentUser()) {
      this._router.navigate(["/register"]);
    }
  }

  public onLogout(): void {
    this._accountService.logout();
    this._router.navigate(["/register"]);
  }
}
