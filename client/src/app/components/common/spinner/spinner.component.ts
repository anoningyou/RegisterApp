import { Component, inject } from '@angular/core';
import { SpinnerService } from '../../../services/spinner.service';
import { MatProgressSpinnerModule, MatSpinner } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-spinner',
  standalone: true,
  imports: [
    MatProgressSpinnerModule
  ],
  templateUrl: './spinner.component.html',
  styleUrl: './spinner.component.scss'
})
export class SpinnerComponent {

  private readonly _spinnerService: SpinnerService = inject(SpinnerService);

  public readonly showSpinner = this._spinnerService.showSpinner;
}
