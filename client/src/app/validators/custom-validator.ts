import { AbstractControl, ValidatorFn } from '@angular/forms';

export class CustomValidators {
  public static matchValues(
    matchTo: string,
    errorCode: string = 'matchValues'
  ): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value
        ? null
        : { [errorCode]: true };
    };
  }
}
