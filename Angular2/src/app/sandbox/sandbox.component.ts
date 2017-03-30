import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
	moduleId: module.id,
	selector: 'sandbox',
	template: `
<!--<form #editForm="ngForm">-->
<!--<even-validator [(ngModel)]="numericValue" [formControl]='test'></even-validator>-->
<!--<datetime-picker placeholder="From" name="test" class="daterange" [hideTime]="true" [(ngModel)]="beginDate" required [selectOnCalendarClick]="true" 
	[minDate]="minBeginDate" [maxDate]="maxDate"></datetime-picker>-->
<input type="text" dateTimePicker [(ngModel)]="beginDate" />
{{beginDate}}
{{beginDate.getTime ? beginDate.getTime() : 0}}
<!--</form>-->
`
})
export class SandboxComponent {
	protected numericValue: number;

	protected minBeginDate = new Date(2010, 1, 1);
	protected maxDate = new Date(2020, 1, 1);
	protected beginDate = new Date();
	constructor() {
		
	}

}