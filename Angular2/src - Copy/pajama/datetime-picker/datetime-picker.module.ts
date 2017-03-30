import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DateTimePickerComponent, DateTimePickerMinValidator, DateTimePickerMaxValidator } from './datetime-picker.component';

@NgModule({
	imports: [
		CommonModule,
		FormsModule,
		ReactiveFormsModule
	],
	declarations: [
		DateTimePickerComponent,
		DateTimePickerMinValidator,
		DateTimePickerMaxValidator
	],
	exports: [
		DateTimePickerComponent
	]
})
export class DateTimePickerModule { }