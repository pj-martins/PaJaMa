import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MultiTextboxComponent } from './multi-textbox.component';
import { MultiTextboxDirective } from './multi-textbox.directive';

@NgModule({
	imports: [
		CommonModule,
		FormsModule
	],
	declarations: [
		MultiTextboxComponent,
		MultiTextboxDirective
	],
	exports: [
		MultiTextboxComponent,
		MultiTextboxDirective
	],
	entryComponents: [
		MultiTextboxComponent
	]
})
export class MultiTextboxModule { }