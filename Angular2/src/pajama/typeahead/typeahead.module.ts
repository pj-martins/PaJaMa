import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TypeaheadComponent } from './typeahead.component';
import { TypeaheadDirective } from './typeahead.directive';
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
		MultiTypeaheadComponent,
		TypeaheadDirective
	],
	exports: [
		TypeaheadComponent,
		MultiTypeaheadComponent,
		TypeaheadDirective
	],
	entryComponents: [
		TypeaheadComponent
	]
})
export class TypeaheadModule { }