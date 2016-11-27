import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MultiSelectTextboxComponent } from './multi-select-textbox.component';
import { TypeaheadModule } from './typeahead.module';
import { ParserService } from './parser.service';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		TypeaheadModule
	],
	declarations: [
		MultiSelectTextboxComponent
	],
	exports: [
		MultiSelectTextboxComponent
	],
	providers: [
		ParserService
	]
})
export class MultiSelectTextboxModule { }