import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { RecipesComponent } from './recipes/recipes.component';
import { ApiService } from './shared/services/api.service';
import { RouterModule } from '@angular/router';
import { TypeaheadModule, OverlayModule } from 'pajama'
import { routing } from './app.routing';

@NgModule({
	imports: [
		BrowserModule,
		FormsModule,
		HttpModule,
		TypeaheadModule,
		OverlayModule,
		routing
	],
	providers: [ApiService],
	declarations: [AppComponent, RecipesComponent],
	bootstrap: [AppComponent]
})
export class AppModule { }
