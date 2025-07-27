import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ApiLinkService {
  baseUrl = 'http://localhost:5210';
  constructor(private http: HttpClient) {}

  get(endpoint: string): Observable<any> {
    return this.http.get(this.baseUrl + '/' + endpoint);
  }

  post(endpoint: string, data: any): Observable<any> {
    return this.http.post(this.baseUrl + '/' + endpoint, data);
  }

  delete(endpoint: string): Observable<any> {
    return this.http.delete(this.baseUrl + '/' + endpoint);
  }

  put(endpoint: string, body: any): Observable<any> {
    return this.http.put(this.baseUrl + '/' + endpoint, body);
  }
  // deleteByEmail(endpoint:string,email:object):Observable<any>{
  //   return this.http.delete(this.baseUrl+'/'+endpoint,email);
  // }
}
