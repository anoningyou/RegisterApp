import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export abstract class BaseHttpService {

  private _baseUrl = environment.apiUrl;

  protected http: HttpClient = inject(HttpClient);

  protected get baseUrl(): string {
    return this._baseUrl;
  }

  constructor() {
      if(!this._baseUrl.endsWith('/'))
        this._baseUrl = `${this._baseUrl}/`
  }

  protected getUrl(action: string): string {
    return `${this.baseUrl}${action}`;
  }
}
