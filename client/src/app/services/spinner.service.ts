import { Injectable, signal, WritableSignal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SpinnerService {

  private readonly _showSpinner: WritableSignal<boolean> = signal(false);

  public readonly showSpinner = this._showSpinner.asReadonly();

  show() {
    this._showSpinner.set(true);
  }

  hide() {
    this._showSpinner.set(false);
  }
}
