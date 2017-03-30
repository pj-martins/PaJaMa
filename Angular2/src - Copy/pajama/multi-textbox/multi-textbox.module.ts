import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MultiTextboxComponent } from './multi-textbox.component';

@NgModule({
	imports: [
		CommonModule,
		FormsModule
	],
	declarations: [
		MultiTextboxComponent
	],
	exports: [
		MultiTextboxComponent
	]
})
export class MultiTextboxModule { }