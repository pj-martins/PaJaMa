import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TypeaheadComponent } from './typeahead.component';
import { TypeaheadDirective } from './typeahead.directive';
import { MultiTypeaheadComponent } from './multi-typeahead.component';
import { MultiTypeaheadDirective } from './multi-typeahead.directive';
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
		TypeaheadDirective,
		MultiTypeaheadDirective
	],
	exports: [
		TypeaheadComponent,
		MultiTypeaheadComponent,
		TypeaheadDirective,
		MultiTypeaheadDirective
	],
	entryComponents: [
		TypeaheadComponent,
		MultiTypeaheadComponent
	]
})
export class TypeaheadModule { }