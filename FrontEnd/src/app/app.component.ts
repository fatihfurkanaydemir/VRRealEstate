import { Component, PLATFORM_ID, Inject } from "@angular/core";
import { isPlatformBrowser } from "@angular/common";
import { LoadingBarService } from "@ngx-loading-bar/core";
import { map, delay, withLatestFrom } from "rxjs/operators";
import { TranslateService } from "@ngx-translate/core";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"],
})
export class AppComponent {
  constructor(@Inject(PLATFORM_ID) private platformId: Object) {}
}
