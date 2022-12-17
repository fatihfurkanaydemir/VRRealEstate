import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class AuthService {
  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<string> {
    // Call the login API, passing in the username and password.
    // The API should return a JWT token if the login is successful.
    return this.http.post<string>("/api/login", { username, password });
  }

  logout(): Observable<void> {
    // Call the logout API.
    return this.http.post<void>("/api/logout", {});
  }

  checkAuth(): Observable<boolean> {
    // Call the check auth API.
    return this.http.get<boolean>("/api/check-auth");
  }
}
