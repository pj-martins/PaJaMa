import { Component, Input, OnInit } from '@angular/core';
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
<!--</form>-->
`
})
export class SandboxComponent implements OnInit {
	protected numericValue: number;

	protected minBeginDate = new Date(2010, 1, 1);
	protected maxDate = new Date(2020, 1, 1);
	protected beginDate: Date = new Date();
	constructor() {
		
	}

	ngOnInit() {
		window.setTimeout(() => this.beginDate = new Date(), 3000);
	}
}