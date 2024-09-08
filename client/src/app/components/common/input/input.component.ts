import { ChangeDetectionStrategy, Component, computed, input, InputSignal, Self, Signal, signal, WritableSignal } from '@angular/core';
import { ControlValueAccessor, FormControl, FormsModule, NgControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { ValueTypeEnum } from '../../../enums/value-type-enum';
import { CommonModule, KeyValue } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import {MatCheckboxModule} from '@angular/material/checkbox';
import { MatIconButton } from '@angular/material/button';

@Component({
  selector: 'app-input',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatFormField,
    MatSelectModule,
    MatLabel,
    MatError,
    MatIcon,
    MatIconButton,
    MatCheckboxModule,
  ],
  templateUrl: './input.component.html',
  styleUrl: './input.component.scss',

})
export class InputComponent implements ControlValueAccessor {

  //#region public properties

  public label: InputSignal<string> = input<string>("");

  public showLabel: InputSignal<boolean> = input<boolean>(true);

  public placeholder: InputSignal<string> = input<string>("");

  public type: InputSignal<ValueTypeEnum> = input<ValueTypeEnum>(ValueTypeEnum.Text);

  public readonly inputType: Signal<string> = computed(() => {
    switch (this.type()) {
      case ValueTypeEnum.Password:
        return "password";
      case ValueTypeEnum.Email:
        return "email";
      default:
        return "text";
    }
  })

  public options: InputSignal<KeyValue<string, string>[]> = input<KeyValue<string, string>[]>([]);

  public readonly hidePassword: WritableSignal<boolean> = signal(true);

  public errorTextFunctions: InputSignal<KeyValue<string, (control: NgControl, label?: string) => string> []>
    = input<KeyValue<string, (control: NgControl, label?: string) => string> []>([
    {
      key: "required",
       value: (control: NgControl, label?: string) => `${label ?? "This"} is a required field`
    },
    {
      key: "minlength",
      value: (control: NgControl, label?: string) => {
        if (!control.errors)
          return "";
        const length: string = control.errors['minlength'].requiredLength?.toString();
        let ending = "s";
        if (length.endsWith("1"))
          ending = "";

        return `${label ?? "Field"} must be at least ${length} caracter${ending}`
      }
    },
    {
      key: "maxlength",
      value: (control: NgControl, label?: string) => {
        if (!control.errors)
          return "";
        const length: string = control.errors['maxlength'].requiredLength?.toString();
        let ending = "s";
        if (length.endsWith("1"))
          ending = "";

        return `${label ?? "Field"} must be at most ${length} caracter${ending}`
      }
    },
    {
      key: "min",
      value: (control: NgControl, label?: string) => `Minimun value is ${control.errors!['min'].min}`
    },
    {
      key: "max",
      value: (control: NgControl, label?: string) => `Maximum value is ${control.errors!['max'].max}`
    },
    {
      key: "email",
      value: (control: NgControl, label?: string) => `${label ?? "Field"} must be a valid email`
    },
  ]);

  public additionalErrorTextFunctions: InputSignal<KeyValue<string, (control: NgControl, label?: string) => string>[]>
    = input<KeyValue<string, (control: NgControl, label?: string) => string> []>([]);

  public readonly allErrorTextFunctions: Signal<KeyValue<string, (control: NgControl, label?: string) => string>[]> =computed(() => {
    return [...this.errorTextFunctions(), ...this.additionalErrorTextFunctions()];
  });

  public get control(): FormControl {
    return this.ngControl.control as FormControl;
  }

  public readonly valueTypeEnum: typeof ValueTypeEnum = ValueTypeEnum;

  //#endregion public properties

  //#region constructor

  constructor (@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }

  //#endregion constructor

  //#region valueAccessor

  writeValue(obj: any): void {

  }

  registerOnChange(fn: any): void {

  }
  registerOnTouched(fn: any): void {

  }
  setDisabledState?(isDisabled: boolean): void {

  }

  //#endregion valueAccessor

  //#region event handlers

  onTogglePasswordVisibility(): void {
    this.hidePassword.update((value) => !value);
  }

  public getErrorMessage(): string {
    if (!this.control.errors)
      return "";

    let error = `${this.label() ?? "Field"} is invalid`;

    const errorFuncs = this.allErrorTextFunctions()
      .filter(x => !!this.control.errors![x.key]);
    if (errorFuncs.length > 0) {
      error = errorFuncs[0].value(this.ngControl, this.label());
    }

    return error;
  }

  //#endregion event handlers

  //#region private methods



  //#endregion private methods

}
