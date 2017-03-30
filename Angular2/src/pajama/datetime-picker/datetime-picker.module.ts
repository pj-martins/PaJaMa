import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DateTimePickerComponent } from './datetime-picker.component';
import { DateTimePickerDirective } from './datetime-picker.directive';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		ReactiveFormsModule
	],
	declarations: [
		DateTimePickerComponent,
		DateTimePickerDirective
	],
	exports: [
		DateTimePickerComponent,
		DateTimePickerDirective
	],
	entryComponents: [
		DateTimePickerComponent
	]
})
export class DateTimePickerModule { }