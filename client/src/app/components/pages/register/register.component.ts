import { Component, computed, DestroyRef, effect, inject, OnInit, Signal, signal, untracked, WritableSignal } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormsModule, NgControl, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatStepperModule } from '@angular/material/stepper';
import { InputComponent } from '../../common/input/input.component';
import { MatLabel } from '@angular/material/form-field';
import { ValueTypeEnum } from 'src/app/enums/value-type-enum';
import { AccountService } from 'src/app/services/account.service';
import { PlacesHttpService } from 'src/app/services/places-http.service';
import { CountryDto } from 'src/app/dto/country-dto';
import { ProvinceDto } from 'src/app/dto/province-dto';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { take } from 'rxjs';
import { RegisterDto } from 'src/app/dto/register-dto';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { KeyValue } from '@angular/common';
import { CustomValidators } from 'src/app/validators/custom-validator';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    MatButtonModule,
    MatStepperModule,
    FormsModule,
    ReactiveFormsModule,
    MatLabel,
    InputComponent
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {

  //#region Private fields

  private readonly _formBuilder: FormBuilder = inject(FormBuilder);

  public readonly valueTypeEnum: typeof ValueTypeEnum = ValueTypeEnum;

  private readonly _accountService: AccountService = inject(AccountService);

  private readonly _placesService: PlacesHttpService = inject(PlacesHttpService);

  private readonly _destroyRef: DestroyRef = inject(DestroyRef);

  private readonly _router: Router = inject(Router);

  private readonly _toastr: ToastrService = inject(ToastrService);

  private readonly _countries: WritableSignal<CountryDto[]> = signal([]);

  private readonly _provinces: WritableSignal<ProvinceDto[]> = signal([]);

  //#region Private fields

  //#region Properties

  public firstFormGroup = this._formBuilder.group({
    login: new FormControl<string | null>(null, [Validators.required, Validators.email]),
    password: new FormControl<string | null>(null, [Validators.required, Validators.pattern(/^(?=[^A-Za-z]*[A-Za-z])(?=\D*\d).{2,}$/)]),
    confirmPassword: new FormControl<string | null>(null, [Validators.required, CustomValidators.matchValues('password')]),
    agreement: new FormControl (false as boolean, [Validators.requiredTrue]),
  });

  public secondFormGroup = this._formBuilder.group({
    country: new FormControl<string | null>(null, [Validators.required]),
    province: new FormControl<string | null>({value: null, disabled: true}, [Validators.required]),
  });

  public readonly additionalErrorPasswordFunctions: KeyValue<string, (control: NgControl, label?: string) => string>[]
  = [
    {
      key: "pattern",
      value: (control: NgControl, label?: string) => `${label ?? "Field"} must contain at least 1 letter and 1 digit`
    },
    {
      key: "matchValues",
      value: (control: NgControl, label?: string) => `${label ?? "Field"} must be the same as password`
    }
  ];

  public readonly countries: Signal<KeyValue<string,string>[]> = computed(() => {
    return this._countries().map((val) => {
      return {
        key: val.id,
        value: val.name
        } as KeyValue<string, string>;
    }
    );
  });

  public readonly provinces: Signal<KeyValue<string,string>[]> = computed(() => {
    return this._provinces().map((val) => {
      return {
        key: val.id,
        value: val.name
        } as KeyValue<string, string>;
    }
    );
  });

  //#endregion Properties

  //#region Event handlers

  public ngOnInit(): void {
    this.reloadCountries();

    if (!this.secondFormGroup.controls.country.value) {
      this.secondFormGroup.controls.province.disable();
    }

    this.secondFormGroup.controls.country.valueChanges.subscribe((_) => {
      if (!this.secondFormGroup.controls.country.value) {
        this.secondFormGroup.controls.province.disable();
      }
      else {
        this.secondFormGroup.controls.province.enable();
      }

      this.reloadProvinces();
    })

    this.firstFormGroup.controls.password.valueChanges.subscribe({
      next: () => this.firstFormGroup.controls.confirmPassword.updateValueAndValidity()
    });

  }

  public onSubmitFirstStep(): void {
    if (!this.firstFormGroup.valid) {
      this.firstFormGroup.markAllAsTouched();
    }
  }

  public onSubmitSecondStep(): void {
    if (!this.secondFormGroup.valid) {
      this.secondFormGroup.markAllAsTouched();
      return;
    }

    const registerDto = {
      userName: this.firstFormGroup.controls.login.value,
      password: this.firstFormGroup.controls.password.value,
      provinceId: this.secondFormGroup.controls.province.value
    } as RegisterDto;

    this._accountService.register(registerDto)
    .pipe(take(1), takeUntilDestroyed(this._destroyRef))
    .subscribe({
      next: (_) => {
        this._router.navigate(["/"]);
      },
      error: (errors) => {
        if (Array.isArray(errors)) {
          errors.forEach((error) => {
            this._toastr.error(error);
          });
        }
      }
    });
  }

  //#endregion Event handlers

  //#region Public methods

  matchValues(matchTo: string, notMatchingKey: string) : ValidatorFn {
    return(control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : {[notMatchingKey]: true}
    }
  }

  //#endregion Public methods

  //#region Private methods

  private reloadCountries(): void {
    if (!!this.secondFormGroup.controls.country.value) {
      this.secondFormGroup.controls.country.setValue(null);
    }
    this._placesService.getCountries()
    .pipe(take(1), takeUntilDestroyed(this._destroyRef))
    .subscribe({
      next: (countries: CountryDto[]) => {
        this._countries.set(countries);
      },
      error: (_) => {
        this._countries.set([]);
      }
    });
  };

  private reloadProvinces(): void {
    if (!!this.secondFormGroup.controls.province.value) {
      this.secondFormGroup.controls.province.setValue(null);
    }

    const countryId = this.secondFormGroup.controls.country.value;
    if (!!countryId) {
      this._placesService.getProvinces(this.secondFormGroup.controls.country.value)
      .pipe(take(1), takeUntilDestroyed(this._destroyRef))
      .subscribe({
        next: (provinces: ProvinceDto[]) => {
          this._provinces.set(provinces);
        },
        error: (_) => {
          this._provinces.set([]);
        }
      });
    }
    else {
      this._provinces.set([]);
    }
  }

  //#endregion Private methods

}
