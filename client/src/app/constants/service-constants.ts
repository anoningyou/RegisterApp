class Account
{
    private _root = 'Account';
    Register = `${this._root}/Register`;
}

class Places
{
    private _root = 'Places';
    GetCountries = `${this._root}/GetCountries`;
    GetProvinces = `${this._root}/GetProvinces`;
}

export class ServiceConstants {
  public static Account: Account = new Account();
  public static Properties: Places = new Places();
}
