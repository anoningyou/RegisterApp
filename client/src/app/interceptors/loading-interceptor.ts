import { HttpErrorResponse, HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { ToastrService } from "ngx-toastr";
import { catchError } from "rxjs";

export const netErrorHandlerInterceptor: HttpInterceptorFn = (request, next) => {

  const toastr = inject(ToastrService);

  return next(request).pipe(
    catchError((error: HttpErrorResponse) =>{
      if(error){
        switch (error.status){
          case 400:
            if(error.error.errors){
              const modelStateErrors = [];
              for (const key in error.error.errors){
                if (error.error.errors[key]) {
                  modelStateErrors.push(error.error.errors[key])
                }
              }
              throw modelStateErrors.flat();
            }
            else {
              toastr.error(error.error, error.status.toString())
            }
            break;
          case 401:
            toastr.error("Unathorized", error.status.toString());
            break;
          case 404:
            toastr.error("Not found", error.status.toString());
            break;
          case 500:
              toastr.error("Server error", error.status.toString());
            break;
          default:
            toastr.error("Something unexpected went wrong");
            console.log(error);
            break;
        }
      }
      throw error;
    })
  );
};
