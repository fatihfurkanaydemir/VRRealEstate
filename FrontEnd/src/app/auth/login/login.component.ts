import { Component, OnInit } from "@angular/core";
import { FormBuilder, Validators, FormGroup } from "@angular/forms";
import { AuthService } from "src/app/shared/services/auth.service";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.scss"],
})
export class LoginComponent implements OnInit {
  public show: boolean = false;
  public loginForm: FormGroup;
  public errorMessage: any;

  constructor(public authService: AuthService, private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      email: ["", [Validators.required, Validators.email]],
      password: ["", [Validators.required, Validators.minLength(5)]],
    });
  }

  ngOnInit() {}

  showPassword() {
    this.show = !this.show;
  }

  login() {
    this.authService
      .login(this.loginForm.value.email, this.loginForm.value.password)
      .subscribe(
        (token) => {
          // Save the JWT token in a cookie or local storage for later use.
        },
        (error) => {
          // Handle the error.
        }
      );
  }
}
