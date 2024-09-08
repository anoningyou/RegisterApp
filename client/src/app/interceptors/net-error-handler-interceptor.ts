import { HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { finalize } from "rxjs";
import { BusyService } from "src/app/services/busy.service";

export const loadingInterceptor: HttpInterceptorFn = (request, next) => {

  const busyService: BusyService = inject(BusyService);

    busyService.busy();
    
    return next(request).pipe(
      finalize(()=>{
        busyService.idle();
      })
    );
};
