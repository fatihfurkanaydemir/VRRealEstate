import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AuthService } from "src/app/shared/services/auth.service";


@Component({
  templateUrl: "./register.component.html",
  styleUrls: ["./register.component.scss"],
})
export class RegisterComponent implements OnInit {
  public show1: boolean = false;
  public show2: boolean = false;
  public registerForm: FormGroup;
  public customerList = [];

  constructor(

    public aServ: AuthService,
    private fb: FormBuilder,

  ) {
    this.registerForm = this.fb.group(
      {
        firstName: ["", [Validators.required]],
        lastName: ["", [Validators.required]],
        username: [
          "",
          [Validators.required, Validators.pattern("^[a-zA-Z -']+")],
        ],
        email: ["", [Validators.required, Validators.email]],
        customerID: [Number, [Validators.required]],
        password: [
          "",
          [
            Validators.required,
            Validators.minLength(5),
            Validators.maxLength(30),
          ],
        ],
        repassword: [
          "",
          [
            Validators.required,
            Validators.minLength(5),
            Validators.maxLength(30),
          ],
        ],
        areaCode: [
          "",
          [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(3),
          ],
        ],
        phone: [
          "",
          [
            Validators.required,
            Validators.minLength(7),
            Validators.maxLength(7),
          ],
        ],
        agreePrivacy: [false, [Validators.required]],
      },
      {
        validator: this.checkRepassword("password", "repassword"),
      }
    );
  }

  checkRepassword(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];

      if (
        matchingControl.errors &&
        !matchingControl.errors.confirmedValidator
      ) {
        return;
      }

      if (control.value !== matchingControl.value) {
        matchingControl.setErrors({ confirmedValidator: true });
      } else {
        matchingControl.setErrors(null);
      }
    };
  }

  ngOnInit() {
    
  }

  showPassword1() {
    this.show1 = !this.show1;
  }
  showPassword2() {
    this.show2 = !this.show2;
  }
  register() {
    if (this.registerForm.value.agreePrivacy) {
      //this.aServ.register(this.registerForm.value.email, this.registerForm.value.password, this.registerForm.value.username, this.registerForm.value.firstName + " " + this.registerForm.value.lastName, this.registerForm.value.areaCode + "" + this.registerForm.value.phone, this.registerForm.value.customerID);
    } else {
      alert("Üye olmak için gizlilik sözleşmesini onaylamalısınız!");
      this.registerForm.reset();
    }
  }
}
