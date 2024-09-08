import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ServiceConstants } from 'src/app/constants/service-constants';
import { BaseHttpService } from 'src/app/services/base-http.service';
import { CountryDto } from 'src/app/dto/country-dto';
import { ProvinceDto } from 'src/app/dto/province-dto';

@Injectable({
  providedIn: 'root'
})
export class PlacesHttpService extends BaseHttpService {

  getCountries(nameFilter?: string | null) : Observable<CountryDto[]> {
    let params = new HttpParams();
    if (!!nameFilter) {
      params = params.append('nameFilter', nameFilter);
    }

    return this.http.get<CountryDto[]>(
      this.getUrl(ServiceConstants.Properties.GetCountries),
      { params: params}
    );
  }

  getProvinces(countryId?: string | null, nameFilter?: string | null) : Observable<ProvinceDto[]> {
    let params = new HttpParams();
    if (!!countryId) {
      params = params.append('countryId', countryId);
    }

    if (!!nameFilter) {
      params = params.append('nameFilter', nameFilter);
    }

    return this.http.get<ProvinceDto[]>(
      this.getUrl(ServiceConstants.Properties.GetProvinces),
      { params: params}
    );
  }
}
