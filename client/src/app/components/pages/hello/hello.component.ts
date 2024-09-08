import { Component, computed, inject, Signal } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-hello',
  standalone: true,
  imports: [],
  templateUrl: './hello.component.html',
  styleUrl: './hello.component.scss'
})
export class HelloComponent {

  private readonly _accountService: AccountService = inject(AccountService);

  public readonly greeting: Signal<string> = computed(
    () => `Hello, ${this._accountService.currentUser()?.userName ?? ""}!`
  )

}
