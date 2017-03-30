import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TypeaheadComponent } from './typeahead.component';
import { MultiTypeaheadComponent } from './multi-typeahead.component';
import { MultiTextboxModule } from '../multi-textbox/multi-textbox.module';
import { ParserService } from '../services/parser.service';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		MultiTextboxModule
	],
	providers: [ParserService],
	declarations: [
		TypeaheadComponent,
		MultiTypeaheadComponent
	],
	exports: [
		TypeaheadComponent,
		MultiTypeaheadComponent
	]
})
export class TypeaheadModule { }