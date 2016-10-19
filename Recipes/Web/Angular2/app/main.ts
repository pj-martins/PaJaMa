import { bootstrap }    from '@angular/platform-browser-dynamic';
import { provide } from '@angular/core';
import { ROUTER_PROVIDERS } from '@angular/router';
import { HTTP_PROVIDERS } from '@angular/http';
import { PlatformLocation, Location, LocationStrategy, HashLocationStrategy, PathLocationStrategy, APP_BASE_HREF} from '@angular/common';
import { AppComponent } from './app.component';

bootstrap(AppComponent, [HTTP_PROVIDERS, ROUTER_PROVIDERS, provide(LocationStrategy, { useClass: HashLocationStrategy })]);