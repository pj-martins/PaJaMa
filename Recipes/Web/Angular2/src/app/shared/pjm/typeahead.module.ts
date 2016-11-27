import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TypeaheadComponent } from './typeahead.component';
import { ParserService } from './parser.service';

@NgModule({
	imports: [
		CommonModule,
		FormsModule
	],
	declarations: [
		TypeaheadComponent,
	],
	exports: [
		TypeaheadComponent
	],
	providers: [
		ParserService
	]
})
export class TypeaheadModule { }