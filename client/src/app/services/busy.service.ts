import { inject, Injectable } from '@angular/core';
import { SpinnerService } from 'src/app/services/spinner.service';

@Injectable({
  providedIn: 'root'
})
export class BusyService {

  private _spinnerService: SpinnerService = inject(SpinnerService);
  private _busyRequestsCount: number = 0;

  public busy(){
    this._busyRequestsCount++;
    this._spinnerService.show()
  }

  public idle(){
    this._busyRequestsCount--;
    if(this._busyRequestsCount<=0){
      this._busyRequestsCount=0;
      this._spinnerService.hide();
    }
  }
}
