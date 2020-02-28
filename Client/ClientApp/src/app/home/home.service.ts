import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';


export class CompanyModel {
    id: number;
    name: string;
    exchange: string;
    ticker: string;
    isin: string;
    website: string;
}

export class UserModel {
  id: number;
  username: string;
  password: string;
}

@Injectable({
    providedIn: 'root'
})

export class HomeService {
  private readonly _companies: BehaviorSubject<any> = new BehaviorSubject(undefined);
  public readonly companies$: Observable<Array<CompanyModel>> = this._companies.asObservable();
  private url = `https://localhost:44377/api/Companies`;


  constructor(
    private readonly httpClient: HttpClient
  ) {}

  getCompanies(): void {
    this.httpClient.get<any>(this.url).subscribe(companies => {
      this._companies.next(companies) ;
    });
  }

  getCompanyByIsin(isin: String): Observable<CompanyModel> {
    const url = `${this.url}/GetCompanyByIsin?isin=${isin}`;

    return this.httpClient.get<any>(url);
  }

  addCompanies(companyModel: CompanyModel): Observable<CompanyModel> {

    return this.httpClient.post<any>(this.url, companyModel);
  }

  updateCompany(companyModel: CompanyModel): Observable<CompanyModel> {
    const url = `${this.url}/${companyModel.id}`;

    return this.httpClient.put<any>(url, companyModel);
  }

  deleteCompany(companyModel: CompanyModel): Observable<CompanyModel> {
    const url = `${this.url}/${companyModel.id}`;

    return this.httpClient.delete<any>(url);
  }

  getToken(): void {
    const user = new UserModel();
    user.username = 'clientApp';
    user.password = 'angular8Pwd';
    const reqHeader = new HttpHeaders({'No-Auth': 'True' });
    const options = { responseType: 'text' as 'json', headers: reqHeader };
    this.httpClient.post<any>('https://localhost:44377/api/Token', user, options).subscribe(token => {
      window.sessionStorage.setItem('userToken', token);
      this.getCompanies();
    });
 }
}
