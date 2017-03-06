import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { ParserService } from '../pajama/services/parser.service';
import { AppComponent } from './app.component';
import { GridViewModule } from '../pajama/gridview/gridview.module';
import { CheckListModule } from '../pajama/checklist/checklist.module';
import { OverlayModule } from '../pajama/overlay/overlay.module';
import { TypeAheadModule } from '../pajama/typeahead/typeahead.module';
import { PipesModule } from '../pajama/pipes/pipes.module';
import { routing } from './app.routing';
import { GridViewBasicDemoComponent } from './gridview/gridviewbasicdemo.component';

@NgModule({
	imports: [
		BrowserModule,
		FormsModule,
		HttpModule,
		GridViewModule,
		OverlayModule,
		TypeAheadModule,
		PipesModule,
		CheckListModule,
		routing
	],
	providers: [ParserService],
	declarations: [
		AppComponent,
		GridViewBasicDemoComponent
	],
	bootstrap: [AppComponent]
})
export class AppModule { }